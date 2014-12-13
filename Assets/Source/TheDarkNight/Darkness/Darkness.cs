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
        private Vector3 roomEntryPosition;
        private Room nextRoom;

        [Inject]
        public IObservableTime Time { get; set; }

        [Inject]
        public GameObjectInstantiator GOI { get; set; }

        [SerializeField]
        private Room startingRoom;

        [SerializeField]
        private float updateFrequency;

        [SerializeField]
        private float startWaitTime = 2;

        [SerializeField]
        private float maxSpeed = 1;

        [SerializeField]
        private float minSpeed = 0.5f;

        public void SetValues(Room startingRoom) {
            this.startingRoom = startingRoom;
        }
        
        private void Start() {
                nextRoom = startingRoom;
                SetNextRoomEntry();
                updateSubscription = Time.ElapsedIntervals(1 / updateFrequency).Subscribe(_ => Move());
        }

        private void Move() {
            transform.position = Vector3.MoveTowards(transform.position, roomEntryPosition, UnityEngine.Random.Range(maxSpeed, minSpeed));

            if(transform.position == roomEntryPosition && nextRoom.possessed == false && !nextRoom.enlightened) {
                IEnumerable<Room> adjacentRooms = nextRoom.GetAdjacentRooms();
                List<Room> possibleNextRooms = adjacentRooms.Where(r => (!r.enlightened && !r.possessed)).ToList();
                if(possibleNextRooms.Count() > 0) {
                    nextRoom.possessed = true;
                    DuplicateSelf();
                    nextRoom = possibleNextRooms.ElementAt(UnityEngine.Random.Range(0, possibleNextRooms.Count()));
                    SetNextRoomEntry();
                }
            }      
        }

        public void Die() {
            Destroy(this.gameObject);
        }

        public void Hide() {
            this.gameObject.SetActive(false);
        }

        private GameObject DuplicateSelf() {
            GameObject copy = GOI.Instantiate(this.gameObject, this.transform.position, Quaternion.identity);
            copy.transform.parent = this.transform.parent;
            copy.transform.position = this.transform.position;
            copy.GetComponent<Darkness>().SetValues(this.nextRoom);
            return copy;
        }

        private void SetNextRoomEntry() {
            roomEntryPosition = nextRoom.GetEntryPositions().GetNearestToPosition(nextRoom.transform.position);//this.transform.position);
        }

        private void OnDestroy() {
            updateSubscription.Dispose();
        }
    }
}