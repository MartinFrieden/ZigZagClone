using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using Game.Signals;
using UI;
using UI.StatesView;
using UnityEngine;
using Zenject;

public class EnterPoint : MonoBehaviour
{
    [SerializeField] private List<UIPanel> uiPanels;
    [Inject] private SignalBus _signalBus;
    
    void Start()
    {
        _signalBus.Subscribe<ChangeGameStateSignal>(ChangeUIState);
        
        _signalBus.Fire(new ChangeGameStateSignal(GameState.MainMenuState, GameState.None));
    }

    private void OnDisable()
    {
        _signalBus.TryUnsubscribe<ChangeGameStateSignal>(ChangeUIState);
    }

    private void ChangeUIState(ChangeGameStateSignal signal)
    {
        SwitchUIState(signal.ActivatedState);
    }

    private void SwitchUIState(GameState signalActivatedState)
    {
        foreach (var panel in uiPanels)
        {
            if (panel.GameState == signalActivatedState)
            {
                panel.Init();
            }
            else
            {
                panel.Deactivate();
            }
        }
    }
}
