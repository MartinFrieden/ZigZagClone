using Enums;
using UnityEngine;
using Zenject;

namespace UI
{
    public abstract class UIPanel : MonoBehaviour
    {
        [Inject] protected SignalBus signalBus;
        public GameState GameState;
        public abstract void Init();
        public abstract void Deactivate();
    }
}