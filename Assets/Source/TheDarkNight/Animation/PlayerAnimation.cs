using ModestTree.Zenject;
using TheDarkNight.Extensions;
using TheDarkNight.Lights;
using TheDarkNight.Movement;
using TheDarkNight.Observables.Time;
using TheDarkNight.Picking;
using TheDarkNight.PlayerController;
using UniRx;
using UnityEngine;

namespace TheDarkNight.Animation {

    [RequireComponent(typeof(Animator))]
    internal class PlayerAnimation : MonoBehaviour {
        private Animator animator;

        [Inject]
        public IObservableTime Time { get; set; }

        private float depthDirection;
        private float horizontalDirection;

        [SerializeField]
        private RuntimeAnimatorController normalAnimation;

        [SerializeField]
        private RuntimeAnimatorController lightBulbAnimation;

        private void Start() {
            animator = this.TryGetComponent<Animator>();
            IMovement movement = this.TryGetClassInParent<IMovement>();
            IPicker picker = this.TryGetClassInParent<IPicker>();
            ILightBulbInserter inserter = this.TryGetClassInParent<ILightBulbInserter>();

            animator.runtimeAnimatorController = normalAnimation;
            SetState(AnimationState.IDLE);

            movement.HorizontalMovement.Subscribe(HandleHorizontalMovement);
            movement.DepthMovement.Subscribe(HandleDepthMovement);
            picker.Picking.Subscribe(HandlePicking);
            inserter.InsertedLightBulb.Subscribe(_ => {
                transform.parent.GetComponent<MovementController>().enabled = false;

                SetState(AnimationState.INSERT_LIGHT_BULB);
                
                Time.Once(2.0f).Subscribe(__ => {
                    animator.runtimeAnimatorController = normalAnimation;
                    transform.parent.GetComponent<MovementController>().enabled = true;
                });

            });
        }

        private void HandlePicking(IPickable pickable) {
            transform.parent.GetComponent<MovementController>().enabled = false;
            SetState(AnimationState.PICKING);

            Time.Once(1.8f).Subscribe(_ => {
                if(pickable is ILightBulb)
                    animator.runtimeAnimatorController = lightBulbAnimation;

                SetState(AnimationState.IDLE);
                transform.parent.GetComponent<MovementController>().enabled = true;
            });
        }

        private void HandleHorizontalMovement(float direction) {
            horizontalDirection = direction;
            if(direction != 0) {
                    SetState(AnimationState.RUN);
                
                if(direction > 0)
                    transform.localRotation = Quaternion.Euler(0, 90, 0);
                else if(direction < 0)
                    transform.localRotation = Quaternion.Euler(0, 270, 0);
            }
            else if(depthDirection == 0 && GetState() == AnimationState.RUN) {
                SetState(AnimationState.IDLE);
            }
        }

        private void HandleDepthMovement(float direction) {
            depthDirection = direction;
            if(direction != 0) {
                    SetState(AnimationState.RUN);

                if(direction > 0)
                    transform.localRotation = Quaternion.Euler(0, 0, 0);
                else if(direction < 0)
                    transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
            else if(horizontalDirection == 0 && GetState() == AnimationState.RUN)
                SetState(AnimationState.IDLE);
        }

        private AnimationState GetState() {
            return (AnimationState) animator.GetInteger("state");
        }

        private void SetState(AnimationState state) {
            animator.SetInteger("state", (int) state);
        }

        public enum AnimationState {
            IDLE = 0,
            RUN = 1,
            PICKING = 2,
            INSERT_LIGHT_BULB = 3
        }
    }
}