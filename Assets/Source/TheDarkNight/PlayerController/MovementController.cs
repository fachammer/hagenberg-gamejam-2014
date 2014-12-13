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

        [SerializeField]
        private string horizontalMovementAxis;

        [SerializeField]
        private string depthMovementAxis;

        private IMovement playerMovement;

        private void Start() {
            playerMovement = this.TryGetClass<IMovement>();
            IAxesManager axesManager = this.TryGetClass<IAxesManager>();
            IObservableAxis horizontalMovement = axesManager.GetAxis(horizontalMovementAxis);
            IObservableAxis depthMovement = axesManager.GetAxis(depthMovementAxis);

            horizontalMovement.Subscribe(HandleHorizontalAxis).AddTo(movementSubscriptions);
            depthMovement.DistinctUntilChanged().Subscribe(HandleDepthAxis).AddTo(movementSubscriptions);
        }

        private void OnDisable() {
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