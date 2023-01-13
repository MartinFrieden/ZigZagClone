using Game.Signals;
using UnityEngine;
using Zenject;

namespace Game.Platforms
{
    public class StartPlatform : MonoBehaviour
    {
        [Inject] private SignalBus _signalBus;
        
        void Start()
        {
        }
        
    }
}
