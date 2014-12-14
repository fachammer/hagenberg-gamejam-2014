using TheDarkNight.Extensions;
using TheDarkNight.Observables.Input;
using UniRx;
using UnityEngine;

namespace TheDarkNight.GameManager {

    [RequireComponent(typeof(IGameManager))]
    [RequireComponent(typeof(IAxesManager))]
    public class GameController : MonoBehaviour {
        private CompositeDisposable axisSubscriptions = new CompositeDisposable();

        [SerializeField]
        private string pauseAxisName;

        [SerializeField]
        private string pauseJoystickAxisName;

        [SerializeField]
        private string quitAxisName;

        [SerializeField]
        private string quitJoystickAxisName;

        [SerializeField]
        private string startAxisName;

        [SerializeField]
        private string startJoystickAxisName;

        [SerializeField]
        private string restartAxisName;

        [SerializeField]
        private string restartJoystickAxisName;

        private IGameManager gameManager;

        private void Start() {
            gameManager = this.TryGetClass<IGameManager>();
            IAxesManager axesManager = this.TryGetClass<IAxesManager>();

            IObservableAxis pauseAxis = axesManager.GetAxis(pauseAxisName);
            IObservableAxis pauseAxisJoystick = axesManager.GetAxis(pauseJoystickAxisName);
            if(Input.GetJoystickNames().Length < 2)
                pauseAxis.DistinctUntilChanged().Subscribe(HandlePauseAxis).AddTo(axisSubscriptions);
            else
                pauseAxisJoystick.DistinctUntilChanged().Subscribe(HandlePauseAxis).AddTo(axisSubscriptions);

            IObservableAxis quitAxis = axesManager.GetAxis(quitAxisName);
            IObservableAxis quitAxisJoystick = axesManager.GetAxis(quitJoystickAxisName);
            if(Input.GetJoystickNames().Length < 2)
                quitAxis.DistinctUntilChanged().Subscribe(HandleQuitAxis).AddTo(axisSubscriptions);
            else
                quitAxisJoystick.DistinctUntilChanged().Subscribe(HandleQuitAxis).AddTo(axisSubscriptions);

            IObservableAxis startAxis = axesManager.GetAxis(startAxisName);
            IObservableAxis startAxisJoystick = axesManager.GetAxis(startJoystickAxisName);

            if(Input.GetJoystickNames().Length < 2)
                startAxis.DistinctUntilChanged().Subscribe(HandleStartAxis).AddTo(axisSubscriptions);
            else
                startAxisJoystick.DistinctUntilChanged().Subscribe(HandleStartAxis).AddTo(axisSubscriptions);

            IObservableAxis restartAxis = axesManager.GetAxis(restartAxisName);
            IObservableAxis restartAxisJoystick = axesManager.GetAxis(restartJoystickAxisName);
            if(Input.GetJoystickNames().Length < 2)
                restartAxis.DistinctUntilChanged().Subscribe(HandleRestartAxis).AddTo(axisSubscriptions);
            else
                restartAxisJoystick.DistinctUntilChanged().Subscribe(HandleRestartAxis).AddTo(axisSubscriptions);
        }

        private void OnDisable() {
            axisSubscriptions.Clear();
        }

        private void HandleRestartAxis(float axisValue) {
            if(axisValue > 0)
                gameManager.RestartGame();
        }

        private void HandleStartAxis(float axisValue) {
            if(axisValue > 0)
                gameManager.StartGame();
        }

        private void HandleQuitAxis(float axisValue) {
            if(axisValue > 0)
                gameManager.QuitGame();
        }

        private void HandlePauseAxis(float axisValue) {
            if(axisValue > 0)
                gameManager.TogglePauseGame();
        }
    }
}