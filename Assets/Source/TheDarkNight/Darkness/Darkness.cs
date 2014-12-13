using UnityEngine;
using System.Collections;
using ModestTree.Zenject;
using TheDarkNight.Observables.Time;
using System;
using System.Linq;
using UniRx;
using TheDarkNight.Rooms;
using TheDarkNight.Extensions;
using System.Collections.Generic;

namespace TheDarkNight.Darkness {

    public class Darkness : MonoBehaviour, IDarkness {
        private IDisposable updateSubscription = Disposable.Empty;
        public Transform nextRoomEntry;
        public Room nextRoom;
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
        private float minSpeed = 0.5f;

        [SerializeField]
        private float instantiateDistance = 1f;

        [SerializeField]
        private float startWaitSeconds = 1f;

        public void SetValues(Transform startingEntry) {
            this.startingEntry = startingEntry;
        }

        private void Start() {
            Time.Once(startWaitSeconds).Subscribe(_ => {
                nextRoomEntry = startingEntry;
                nextRoom = nextRoomEntry.GetComponentInParent<Room>();
                lastPos = transform.position;
                updateSubscription = Time.ElapsedIntervals(1 / updateFrequency).Subscribe(__ => Move());
            });
        }

        private void Move() {
            if(Vector3.Distance(transform.position, lastPos) > instantiateDistance) {
                lastPos = transform.position;
                Instantiate(darknessDummyPrefab, this.transform.position, this.transform.rotation);
            }

            transform.position = Vector3.MoveTowards(transform.position, nextRoomEntry.position, UnityEngine.Random.Range(maxSpeed, minSpeed));

            if(transform.position == nextRoomEntry.position && !nextRoom.possessed && !nextRoom.enlightened) {
                IEnumerable<Transform> entriesToAdjacentRooms = nextRoom.GetAdjacentRoomsEntries();
                List<Transform> possibleNextEntries = entriesToAdjacentRooms.Where(r => !r.GetComponentInParent<Room>().enlightened && !r.GetComponentInParent<Room>().possessed).ToList();

                if(possibleNextEntries.Count() > 0) {
                    nextRoom.possessed = true;

                    DuplicateSelf(this.nextRoomEntry);

                    nextRoomEntry = possibleNextEntries.ElementAt(UnityEngine.Random.Range(0, possibleNextEntries.Count()));
                    nextRoom = nextRoomEntry.GetComponentInParent<Room>();                    
                }
            }      
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

        public void Hide() {
            this.gameObject.SetActive(false);
        }

        private void OnDestroy() {
            updateSubscription.Dispose();
        }
    }
}