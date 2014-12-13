using UnityEngine;

namespace TheDarkNight.Movement {

    public class Stair : MonoBehaviour {

        private void OnCollisionEnter(Collision collision) {
            Rigidbody rigidBody = collision.collider.rigidbody;
            if(rigidBody != null)
                rigidBody.useGravity = false;

            PlayerMovementBehaviour movement = collision.collider.GetComponent<PlayerMovementBehaviour>();
            if(movement != null)
                movement.movementSettings.maxHorizontalSpeed *= 2;
        }

        private void OnCollisionExit(Collision collision) {
            Rigidbody rigidBody = collision.collider.rigidbody;
            if(rigidBody != null) {
                rigidBody.useGravity = true;
                rigidBody.velocity = new Vector3(rigidBody.velocity.x, 0, rigidBody.velocity.z);
            }

            PlayerMovementBehaviour movement = collision.collider.GetComponent<PlayerMovementBehaviour>();
            if(movement != null)
                movement.movementSettings.maxHorizontalSpeed /= 2;
        }
    }
}