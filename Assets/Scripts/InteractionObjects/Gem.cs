using System;
using Enums;
using Game.Signals;
using Lean.Pool;
using OSSC;
using UnityEngine;
using Zenject;

namespace InteractionObjects
{
    public class Gem : MonoBehaviour
    {
        [Inject] private SignalBus _signalBus;
        [Inject] private SoundController _soundController;
        private PlaySoundSettings _soundSettings;
        private void OnEnable()
        {
            if(_signalBus!=null)
                _signalBus.Subscribe<ChangeGameStateSignal>(OnChangeGameStateSignal);
        }

        private void Start()
        {
            _signalBus.Subscribe<ChangeGameStateSignal>(OnChangeGameStateSignal);
            _soundSettings = new PlaySoundSettings();
            _soundSettings.Init();
            _soundSettings.isLooped = false;
            _soundSettings.name = "CoinPickUp";
        }

        private void OnDisable()
        {
            _signalBus.TryUnsubscribe<ChangeGameStateSignal>(OnChangeGameStateSignal);
        }

        private void OnChangeGameStateSignal(ChangeGameStateSignal signal)
        {
            if(signal.ExitedState == GameState.GameOverState)
                LeanPool.Despawn(this);
        }

        private void OnTriggerEnter(Collider other)
        {
            _soundController.Play(_soundSettings);
            _signalBus.Fire(new TakeGemSignal(1));
            LeanPool.Despawn(this);
        }
    }
}
