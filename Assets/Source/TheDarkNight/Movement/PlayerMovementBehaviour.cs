using TheDarkNight.Extensions;
using UniRx;
using UnityEngine;

namespace TheDarkNight.Movement {

    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Rigidbody))]
    internal class PlayerMovementBehaviour : MonoBehaviour, IMovement {
        public PlayerMovement.Settings movementSettings;

        private PlayerMovement playerMovement;

        public IObservable<float> HorizontalMovement {
            get { return playerMovement.HorizontalMovement; }
        }

        public IObservable<float> DepthMovement {
            get { return playerMovement.DepthMovement; }
        }

        public void MoveHorizontally(float movementScale) {
            playerMovement.MoveHorizontally(movementScale);
        }

        public void MoveDepth(float movementScale) {
            playerMovement.MoveDepth(movementScale);
        }

        private void Awake() {
            Rigidbody r = this.TryGetComponent<Rigidbody>();
            playerMovement = new PlayerMovement(r, audio, movementSettings);
        }

        private void Update() {
            playerMovement.Update();
        }
    }
}