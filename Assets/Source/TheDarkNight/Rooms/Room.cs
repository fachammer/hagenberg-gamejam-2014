using System.Collections.Generic;
using TheDarkNight.Extensions;
using TheDarkNight.Lights;
using UniRx;
using UnityEngine;

namespace TheDarkNight.Rooms {

    [RequireComponent(typeof(Collider))]
    public class Room : MonoBehaviour, IRoom {
        public bool enlightened = false;

        [SerializeField]
        private LightSource lightSource;

        private HashSet<Darkness.Darkness> darknessInTrigger = new HashSet<Darkness.Darkness>();

        [SerializeField]
        private List<Transform> adjacentRoomsEntries;

        public IEnumerable<Transform> GetAdjacentRoomsEntries() {
            return adjacentRoomsEntries;
        }

        public void ClearDarkness() {
            darknessInTrigger.Do(d => { if(d != null) d.Die(); });
            darknessInTrigger.Clear();
        }

        private void Start() {
            collider.isTrigger = true;
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
    }
}