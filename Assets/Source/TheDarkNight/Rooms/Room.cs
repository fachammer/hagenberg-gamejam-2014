using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using TheDarkNight.Extensions;
using TheDarkNight.Lights;
using UniRx;

namespace TheDarkNight.Rooms {

    public class Room : MonoBehaviour, IRoom {
        [SerializeField]
        private LightSource lightSource;

        public bool enlightened = false;
        public bool possessed = false;

        [SerializeField]
        private List<Room> adjacentRooms;

        [SerializeField]
        private List<Transform> adjacentRoomEntries;

        [SerializeField]
        private List<Transform> roomEntries;

        public IEnumerable<Room> GetAdjacentRooms() {
            return adjacentRooms;
        }

        public IEnumerable<Vector3> GetEntryPositions() {
            return roomEntries.Select(t => t.position).ToList();
        }
        
        private void Start() {
            //lightSource.TurnedOn.Subscribe(_ => { enlightened = true; possessed = false; });      //TODO UNCOMMENT
        }
    }
}
