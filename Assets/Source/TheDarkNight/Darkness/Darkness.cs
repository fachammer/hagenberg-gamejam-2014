using UnityEngine;
using System.Collections;
using ModestTree.Zenject;
using TheDarkNight.Observables.Time;
using TheDarkNight.Extensions;
using System;
using System.Linq;
using UniRx;
using TheDarkNight.Rooms;
using TheDarkNight.Extensions;
using System.Collections.Generic;

namespace TheDarkNight.Darkness {

    [RequireComponent(typeof(Rigidbody))]
    public class Darkness : MonoBehaviour, IDarkness {
        private IDisposable updateSubscription = Disposable.Empty;
        private IDisposable subscription = Disposable.Empty;
        private Transform nextRoomEntry;
        private Room nextRoom;
        private Vector3 lastPos;

        [SerializeField]
        private GameObject darknessDummyPrefab;

        [Inject]
        public IObservableTime Time { get; set; }

        [Inject]
        public GameObjectInstantiator GOI { get; set; }

        [SerializeField]
        private Transform startingEntry;

        [SerializeField]
        private float updateFrequency;

        [SerializeField]
        private float maxSpeed = 1;

        [SerializeField]
        private float rotateSpeed = 5;

        [SerializeField]
        private float minSpeed = 0.5f;

        [SerializeField]
        private float instantiateDistance = 1f;

        [SerializeField]
        private float startWaitSeconds = 1f;

        public void SetValues(Transform startingEntry) {
            this.startingEntry = startingEntry;
        }

        private void Update() {
            transform.Rotate(0, rotateSpeed * UnityEngine.Time.deltaTime, 0);
        }

        private void Move() {
            if(Vector3.Distance(transform.position, lastPos) > instantiateDistance) {
                lastPos = transform.position;
                GameObject newDarkness = GOI.Instantiate(darknessDummyPrefab, this.transform.position, Quaternion.identity);
                newDarkness.transform.position = this.transform.position;
            }

            transform.position = Vector3.MoveTowards(transform.position, nextRoomEntry.position, UnityEngine.Random.Range(maxSpeed, minSpeed));

            if(transform.position == nextRoomEntry.position && RoomEnterable(nextRoom)) {
                IEnumerable<Transform> entriesToAdjacentRooms = nextRoom.GetAdjacentRoomsEntries();
                List<Transform> possibleNextEntries = entriesToAdjacentRooms.Where(r => RoomEnterable(r.GetComponentInParent<Room>())).ToList();

                if(possibleNextEntries.Count() > 0) {
                    nextRoom.possessed = true;

                    DuplicateSelf(this.nextRoomEntry);

                    nextRoomEntry = possibleNextEntries.ElementAt(UnityEngine.Random.Range(0, possibleNextEntries.Count()));
                    nextRoom = nextRoomEntry.GetComponentInParent<Room>();                    
                }
            }      
        }

        private bool RoomEnterable(Room room) {
            return !room.enlightened && !room.possessed;
        }

        private void DuplicateSelf(Transform nextEntry) {
            GameObject copy = GOI.Instantiate(this.gameObject, this.transform.position, Quaternion.identity);
            copy.transform.parent = this.transform.parent;
            copy.transform.position = this.transform.position;
            copy.GetComponent<Darkness>().SetValues(nextEntry);
        }

        public void Die() {
            Destroy(this.gameObject);
        }

        public void SetHidden(bool hidden) {
            this.enabled = !hidden;
            this.GetComponent<MeshRenderer>().enabled = !hidden;
        }

        private void OnDestroy() {
            updateSubscription.Dispose();            
            subscription.Dispose();
        }

        private void OnDisable() {
            updateSubscription.Dispose();
            subscription.Dispose();
        }

        private void Start() {
            subscription = Time.Once(startWaitSeconds).Subscribe(_ => {
                nextRoomEntry = startingEntry;
                nextRoom = nextRoomEntry.GetComponentInParent<Room>();
                lastPos = transform.position;
                updateSubscription = Time.ElapsedIntervals(1 / updateFrequency).Subscribe(__ => Move());
            });
        }

        private void OnEnable() {
            if(Time != null)
                Start();
        }

        private void OnCollisionEnter(Collision other) {
            if(other.collider.GetComponent<Killable>() != null) {
                audio.Play();
                other.collider.GetComponent<Killable>().Kill();
            }
        }
    }
}