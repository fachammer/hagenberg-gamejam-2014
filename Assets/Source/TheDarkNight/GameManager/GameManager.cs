using PRO3.Utility;
using UnityEngine;

namespace TheDarkNight.GameManager {

    public class GameManager : MonoBehaviour, IGameManager {
        private ObservableProperty<bool> gameStarted = new ObservableProperty<bool>(false);
        private ObservableProperty<bool> gameEnded = new ObservableProperty<bool>(false);
        private ObservableProperty<bool> gameQuit = new ObservableProperty<bool>(false);
        private ObservableProperty<bool> gamePaused = new ObservableProperty<bool>(false);

        public ObservableProperty<bool> GameStarted {
            get { return gameStarted; }
        }

        public ObservableProperty<bool> GameEnded {
            get { return gameEnded; }
        }

        public ObservableProperty<bool> GameQuit {
            get { return gameQuit; }
        }

        public ObservableProperty<bool> GamePaused {
            get { return gamePaused; }
        }

        public void StartGame() {
            if(!gamePaused && !gameEnded && !gameQuit)
                gameStarted.Value = true;
        }

        public void EndGame() {
            gameEnded.Value = true;
        }

        public void TogglePauseGame() {
            gamePaused.Value = !gamePaused.Value;
        }

        public void QuitGame() {
            if(gamePaused || !gameStarted)
                gameQuit.Value = true;
        }

        public void RestartGame() {
            if(gamePaused)
                Application.LoadLevel(0);
        }
    }
}