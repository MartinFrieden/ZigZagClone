
using Enums;
using Game;
using Game.Signals;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.StatesView
{
    public class UIGameStateView : UIPanel
    {
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private Button pauseButton;
        [SerializeField] private Button mainMenu;
        [Inject] private SignalBus _signalBus;
        [Inject] private GameProgress _gameProgress;
        private bool _gameOnPause;
        public override void Init()
        {
            scoreText.text = "0";
            gameObject.SetActive(true);
            pauseButton.onClick.AddListener(SetPause);
            mainMenu.onClick.AddListener(GoToMainMenu);
            _signalBus.Subscribe<IncreaseScoreSignal>(UpdateScore);
        }

        private void UpdateScore(IncreaseScoreSignal signal)
        {
            scoreText.text = _gameProgress.GetCurrentScore().ToString();
        }

        private void GoToMainMenu()
        {
            _signalBus.Fire(new ChangeGameStateSignal(GameState.MainMenuState, GameState.GameOverState));
        }

        public void SetPause()
        {
            _gameOnPause = !_gameOnPause;
            Time.timeScale = _gameOnPause ? 0 : 1;
        }

        public override void Deactivate()
        {
            pauseButton.onClick.RemoveListener(SetPause);
            mainMenu.onClick.RemoveListener(GoToMainMenu);
            gameObject.SetActive(false);
            _signalBus.TryUnsubscribe<IncreaseScoreSignal>(UpdateScore);
        }
    }
}
