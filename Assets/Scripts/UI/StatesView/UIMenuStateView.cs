
using Enums;
using Game;
using Game.Signals;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.StatesView
{
    public class UIMenuStateView : UIPanel
    {
        [SerializeField] private TextMeshProUGUI bestScore;
        [SerializeField] private TextMeshProUGUI gamesPlayed;
        [SerializeField] private TextMeshProUGUI gemsCount;
        [SerializeField] private Button playButton;
        [Inject] private GameProgress _gameProgress;
        
        public override void Init()
        {
            playButton.onClick.AddListener(()=>
                signalBus.Fire(new ChangeGameStateSignal(GameState.GameState, GameState.MainMenuState)));
            bestScore.text = $"Best Score: {_gameProgress.GetBestScore().ToString()}";
            gamesPlayed.text = $"GamesPlayed: {_gameProgress.GetGamesPlayed().ToString()}";
            gemsCount.text = $"{_gameProgress.GetGemsCount().ToString()}";
            gameObject.SetActive(true);
        }

        public override void Deactivate()
        {
            playButton.onClick.RemoveAllListeners();
            gameObject.SetActive(false);
        }
    }
}
