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
        private List<Transform> adjacentRoomsEntries;

        public IEnumerable<Transform> GetAdjacentRoomsEntries() {
            return adjacentRoomsEntries;
        }
        
        private void Start() {
            lightSource.TurnedOn.Subscribe(_ => { enlightened = true; possessed = false; });
        }
    }
}
