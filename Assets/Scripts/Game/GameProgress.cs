using CW.Common;
using Enums;
using Game.Signals;
using UnityEngine;
using Zenject;

namespace Game
{
    public class GameProgress
    {
        [Inject] private SignalBus _signalBus;

        private int _currentScore;
        
        [Inject]
        public void Construct()
        {
            _currentScore = 0;
            _signalBus.Subscribe<ChangeGameStateSignal>(OnChangeGameState);
            _signalBus.Subscribe<TakeGemSignal>(OnTakeGem);
            _signalBus.Subscribe<IncreaseScoreSignal>(OnIncreaseScore);
        }

        public int GetCurrentScore()
        {
            return _currentScore;
        }
        
        private void OnTakeGem(TakeGemSignal signal)
        {
            PlayerPrefs.SetInt("GemsCount", PlayerPrefs.GetInt("GemsCount",0)+signal.Amount);
        }

        private void OnIncreaseScore()
        {
            _currentScore++;
        }

        private void OnChangeGameState(ChangeGameStateSignal signal)
        {
            if(signal.ActivatedState == GameState.GameState)
                PlayerPrefs.SetInt("GamesPlayed", PlayerPrefs.GetInt("GamesPlayed")+1);
            if (signal.ExitedState == GameState.GameState)
            {
                if (_currentScore > GetBestScore())
                {
                    SetBestScore(_currentScore);
                }
            }

            if (signal.ExitedState == GameState.GameOverState)
                _currentScore = 0;
        }

        public int GetBestScore()
        {
           return PlayerPrefs.GetInt("BestScore",0);
        }

        public void SetBestScore(int amount)
        {
            PlayerPrefs.SetInt("BestScore", amount);
        }

        public int GetGamesPlayed()
        {
            return PlayerPrefs.GetInt("GamesPlayed",0);
        }

        public int GetGemsCount()
        {
            return PlayerPrefs.GetInt("GemsCount", 0);
        }
    }
}