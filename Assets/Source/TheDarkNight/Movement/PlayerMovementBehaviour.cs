using TheDarkNight.Extensions;
using UnityEngine;

namespace TheDarkNight.Movement {

    [RequireComponent(typeof(Rigidbody))]
    internal class PlayerMovementBehaviour : MonoBehaviour, IMovement {
        public PlayerMovement.Settings movementSettings;

        private PlayerMovement playerMovement;

        public void MoveHorizontally(float movementScale) {
            playerMovement.MoveHorizontally(movementScale);
        }

        public void MoveDepth(float movementScale) {
            playerMovement.MoveDepth(movementScale);
        }

        private void Start() {
            Rigidbody r = this.TryGetComponent<Rigidbody>();
            playerMovement = new PlayerMovement(r, movementSettings);
        }

        private void Update() {
            playerMovement.Update();
        }
    }
}