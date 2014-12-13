using PRO3.Utility;

namespace TheDarkNight.GameManager {

    public interface IGameManager {

        ObservableProperty<bool> GameStarted { get; }

        ObservableProperty<bool> GameEnded { get; }

        ObservableProperty<bool> GameQuit { get; }

        ObservableProperty<bool> GamePaused { get; }

        void RestartGame();

        void StartGame();

        void EndGame();

        void TogglePauseGame();

        void QuitGame();
    }
}