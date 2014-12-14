using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using TheDarkNight.Extensions;
using TheDarkNight.Lights;
using UniRx;

namespace TheDarkNight.Rooms {

    [RequireComponent(typeof(Collider))]
    public class Room : MonoBehaviour, IRoom {
        [SerializeField]
        private LightSource lightSource;

        public bool enlightened = false;
        public bool possessed = false;
        private HashSet<Darkness.Darkness> darknessInTrigger = new HashSet<Darkness.Darkness>();

        [SerializeField]
        private List<Transform> adjacentRoomsEntries;

        public IEnumerable<Transform> GetAdjacentRoomsEntries() {
            return adjacentRoomsEntries;
        }
        
        private void Start() {
            lightSource.TurnedOn.Subscribe(_ => enlightened = true);
            lightSource.TurnedOff.Subscribe(_ => enlightened = false);
        }


        private void OnTriggerEnter(Collider other) {
            if(other.GetComponent<Darkness.Darkness>() != null) {
                darknessInTrigger.Add(other.GetComponent<Darkness.Darkness>());
            }
        }

        private void OnTriggerStay(Collider other) {
            if(other.GetComponent<Darkness.Darkness>() != null) {
                darknessInTrigger.Add(other.GetComponent<Darkness.Darkness>());
            }
        }

        public void ClearDarkness() {
            darknessInTrigger.Do(d => d.Die());
            darknessInTrigger.Clear();
            possessed = false;
        }
    }
}
