using System;
using Game.Signals;
using OSSC;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Game.Input
{
    public class TouchDetector : MonoBehaviour, IPointerDownHandler
    {
        [Inject] private SignalBus _signalBus;
        [Inject] private SoundController _soundController;
        private PlaySoundSettings _soundSettings;

        private void Start()
        {
            _soundSettings = new PlaySoundSettings();
            _soundSettings.Init();
            _soundSettings.isLooped = false;
            _soundSettings.name = "Click";
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _soundController.Play(_soundSettings);
            _signalBus.Fire<TouchSignal>();
        }
    }
}
