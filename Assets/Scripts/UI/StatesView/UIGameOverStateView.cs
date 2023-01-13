
using Enums;
using Game;
using Game.Signals;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

namespace UI.StatesView
{
    public class UIGameOverStateView : UIPanel
    {
        [SerializeField] private Button restartButton;
        [SerializeField] private TextMeshProUGUI currentScore;
        [SerializeField] private TextMeshProUGUI bestScore;
        [Inject] private GameProgress _gameProgress;
        public override void Init()
        {
            restartButton.onClick.AddListener(RestartGame);
            SetParams();
            gameObject.SetActive(true);
        }

        private void SetParams()
        {
            currentScore.text = $"Current Score{_gameProgress.GetCurrentScore()}";
            bestScore.text = $"Best Score{_gameProgress.GetBestScore()}";
        }

        private void RestartGame()
        {
            signalBus.Fire(new ChangeGameStateSignal(GameState.MainMenuState, GameState.GameOverState));
        }

        public override void Deactivate()
        {
            restartButton.onClick.RemoveAllListeners();
            gameObject.SetActive(false);
        }
    }
}
