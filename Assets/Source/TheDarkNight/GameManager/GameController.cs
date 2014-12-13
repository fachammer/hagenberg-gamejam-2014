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
        private string quitAxisName;

        [SerializeField]
        private string startAxisName;

        [SerializeField]
        private string restartAxisName;

        private IGameManager gameManager;

        private void Start() {
            gameManager = this.TryGetClass<IGameManager>();
            IAxesManager axesManager = this.TryGetClass<IAxesManager>();

            IObservableAxis pauseAxis = axesManager.GetAxis(pauseAxisName);
            pauseAxis.DistinctUntilChanged().Subscribe(HandlePauseAxis).AddTo(axisSubscriptions);

            IObservableAxis quitAxis = axesManager.GetAxis(quitAxisName);
            quitAxis.DistinctUntilChanged().Subscribe(HandleQuitAxis).AddTo(axisSubscriptions);

            IObservableAxis startAxis = axesManager.GetAxis(startAxisName);
            startAxis.DistinctUntilChanged().Subscribe(HandleStartAxis).AddTo(axisSubscriptions);

            IObservableAxis restartAxis = axesManager.GetAxis(restartAxisName);
            restartAxis.DistinctUntilChanged().Subscribe(HandleRestartAxis).AddTo(axisSubscriptions);
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