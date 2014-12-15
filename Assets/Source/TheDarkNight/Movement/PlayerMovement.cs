using System;
using UniRx;
using UnityEngine;

namespace TheDarkNight.Movement {

    internal class PlayerMovement : IMovement {
        private Rigidbody rigidbody;
        private Settings settings;
        private AudioSource audio;
        private int currentLayer = 0;

        private ISubject<float> horizontalMovement = new Subject<float>();
        private ISubject<float> depthMovement = new Subject<float>();

        public IObservable<float> HorizontalMovement {
            get { return horizontalMovement; }
        }

        public IObservable<float> DepthMovement {
            get { return depthMovement; }
        }

        private float CurrentLayerDepth {
            get { return currentLayer * settings.depthLayerWidth; }
        }

        public PlayerMovement(Rigidbody rigidbody, AudioSource audioSource, Settings settings) {
            this.rigidbody = rigidbody;
            this.settings = settings;
            this.audio = audioSource;
        }

        public void MoveHorizontally(float movementScale) {
            if(movementScale == 0 && !rigidbody.useGravity)
                rigidbody.velocity = new Vector3(0, 0, rigidbody.velocity.z);

            rigidbody.velocity += new Vector3(settings.maxHorizontalSpeed * movementScale, 0, 0);
            ClampVelocity();
            
            horizontalMovement.OnNext(movementScale);
        }

        public void MoveDepth(float movementScale) {
            depthMovement.OnNext(movementScale);
            if(IsPlayerMovingInDepth())
                return;

            currentLayer = Mathf.Clamp(currentLayer + Sign(movementScale), 0, settings.depthLayers - 1);
            if(!IsPlayerNearTargetLayer() && !IsPlayerMovingInDepth() && !IsObstacleInDepthDirection(movementScale)) {
                rigidbody.constraints &= ~RigidbodyConstraints.FreezePositionZ;
                rigidbody.AddForce(new Vector3(0, 0, settings.depthMovementForce * Sign(movementScale)));
            }

        }

        public void Update() {
            if(IsPlayerNearTargetLayer() && IsPlayerMovingInDepth()) {
                rigidbody.constraints |= RigidbodyConstraints.FreezePositionZ;
                rigidbody.position = new Vector3(rigidbody.position.x, rigidbody.position.y, CurrentLayerDepth);
                rigidbody.velocity = new Vector3(rigidbody.velocity.x, rigidbody.velocity.y, 0f);
            }

            if(IsPlayerMovingInDepth() || Mathf.Abs(rigidbody.velocity.x) > 0.1f) {
                if(audio.isPlaying == false)
                    audio.Play();
            }
            else
                audio.Stop();

        }

        private bool IsObstacleInDepthDirection(float direction) {
            float collisionRadius = rigidbody.collider.bounds.size.magnitude / 4;
            float collisionDistance = settings.depthLayerWidth + rigidbody.collider.bounds.size.z / 2;
            return Physics.Raycast(rigidbody.position, new Vector3(0, 0, 10), collisionDistance, settings.depthCollisionLayers);
        }

        private bool IsPlayerMovingInDepth() {
            return Mathf.Abs(rigidbody.velocity.z) > 0.1;
        }

        private int Sign(float value) {
            if(value > 0)
                return 1;
            else if(value < 0)
                return -1;

            return 0;
        }

        private bool IsPlayerNearTargetLayer() {
            if(rigidbody.velocity.z > 0)
                return CurrentLayerDepth - rigidbody.position.z <= 0.1;
            else if(rigidbody.velocity.z < 0)
                return rigidbody.position.z - CurrentLayerDepth <= 0.1;
            else
                return Mathf.Abs(CurrentLayerDepth - rigidbody.position.z) <= 0.1;
        }

        private void ClampVelocity() {
            Vector3 clampedVelocity = rigidbody.velocity;
            clampedVelocity.x = Mathf.Clamp(clampedVelocity.x, -settings.maxHorizontalSpeed, settings.maxHorizontalSpeed);
            clampedVelocity.z = Mathf.Clamp(clampedVelocity.z, -settings.maxDepthMovementSpeed, settings.maxDepthMovementSpeed);
            rigidbody.velocity = clampedVelocity;
        }

        [Serializable]
        public class Settings {
            public float maxHorizontalSpeed = 5;
            public float depthMovementForce = 1000;
            public float maxDepthMovementSpeed = 10;

            [Range(0, 10)]
            public int depthLayers = 2;

            public float depthLayerWidth = 2;
            public LayerMask depthCollisionLayers;
        }
    }
}