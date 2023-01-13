using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using Game.Signals;
using UnityEngine;
using Zenject;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private Transform followingObject;
    [Inject] private SignalBus _signalBus;
    private GameState _gameState;
    private Vector3 _distanceToObject;
    private Vector3 _targetPos;
    private readonly float _coefficient = Mathf.Pow(Mathf.Cos(45*Mathf.Deg2Rad),2);

    void Start()
    {
        _gameState = GameState.MainMenuState;
        _signalBus.Subscribe<ChangeGameStateSignal>(ChangeGameStateCamera);
        _distanceToObject = followingObject.position - transform.position;
    }

    private void ChangeGameStateCamera(ChangeGameStateSignal signal)
    {
        _gameState = signal.ActivatedState;
    }

    private void Update()
    {
        if(_gameState == GameState.GameOverState)
            return;
        UpdateCameraPosition();
    }
    
    private void UpdateCameraPosition()
    {
        _targetPos = followingObject.position - _distanceToObject;
        transform.position = new Vector3((_targetPos.x-_targetPos.z)*_coefficient, 
            _targetPos.y,
            (_targetPos.z-_targetPos.x)*_coefficient);
    }
}
