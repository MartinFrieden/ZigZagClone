using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Input
{
    public class AutoInput : IInputService
    {
        public void OnPointerDown(PointerEventData eventData)
        {
            Debug.Log("[AUTO MODE ACTIVATED]");
        }
    }
}