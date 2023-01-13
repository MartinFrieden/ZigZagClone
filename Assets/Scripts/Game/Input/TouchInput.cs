using Game.Signals;
using OSSC;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Game.Input
{
    public class TouchInput : IInputService
    {
        private SignalBus _signalBus;
        private SoundController _soundController;
        [Inject]
        public void Construct(SignalBus signalBus, SoundController soundController)
        {
            _signalBus = signalBus;
            _soundController = soundController;
            
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            Debug.Log("Touch");
            _signalBus.Fire<TouchSignal>();
        }
    }
}
