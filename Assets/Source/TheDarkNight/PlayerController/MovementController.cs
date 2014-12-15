using TheDarkNight.Extensions;
using TheDarkNight.Movement;
using TheDarkNight.Observables.Input;
using UniRx;
using UnityEngine;

namespace TheDarkNight.PlayerController {

    [RequireComponent(typeof(IAxesManager))]
    [RequireComponent(typeof(IMovement))]
    public class MovementController : MonoBehaviour {
        private CompositeDisposable movementSubscriptions = new CompositeDisposable();

        private bool deactivatedOnce = false;

        [SerializeField]
        private string horizontalMovementKeyboardAxis;

        [SerializeField]
        private string horizontalMovementJoystickAxis;

        [SerializeField]
        private string depthMovementKeyboardAxis;

        [SerializeField]
        private string depthMovementJoystickAxis;

        private IMovement playerMovement;

        private void OnEnable() {
            if(deactivatedOnce)
                Start();
        }

        private void Start() {
            playerMovement = this.TryGetClass<IMovement>();
            IAxesManager axesManager = this.TryGetClass<IAxesManager>();
            IObservableAxis horizontalMovement = axesManager.GetAxis(horizontalMovementKeyboardAxis);
            IObservableAxis horizontalMovementJoystick = axesManager.GetAxis(horizontalMovementJoystickAxis);
            IObservableAxis depthMovement = axesManager.GetAxis(depthMovementKeyboardAxis);
            IObservableAxis depthMovementJoystick = axesManager.GetAxis(depthMovementJoystickAxis);

            if(Input.GetJoystickNames().Length < 2)
                horizontalMovement.Subscribe(HandleHorizontalAxis).AddTo(movementSubscriptions);
            else
                horizontalMovementJoystick.Subscribe(HandleHorizontalAxis).AddTo(movementSubscriptions);

            if(Input.GetJoystickNames().Length < 2)
                depthMovement.DistinctUntilChanged().Subscribe(HandleDepthAxis).AddTo(movementSubscriptions);
            else
                depthMovementJoystick.DistinctUntilChanged().Subscribe(HandleDepthAxis).AddTo(movementSubscriptions);
        }

        private void OnDisable() {
            deactivatedOnce = true;
            movementSubscriptions.Clear();
        }

        private void HandleHorizontalAxis(float axisValue) {
            playerMovement.MoveHorizontally(axisValue);
        }

        private void HandleDepthAxis(float axisValue) {
            playerMovement.MoveDepth(axisValue);
        }
    }
}