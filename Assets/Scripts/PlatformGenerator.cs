using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using Game.Platforms;
using Game.Signals;
using Lean.Pool;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class PlatformGenerator : MonoBehaviour
{
    [SerializeField] private Platform platform = null;
    [SerializeField] private Transform startSpawnPoint;

    [Inject] private SignalBus _signalBus;
    private Platform _currentPlatform;
    private Platform _previousPlatform;
    private Camera _camera;
    private float _minX, _maxX;
    
    void Start()
    {
        _signalBus.Subscribe<SpawnPlatformSignal>(SpawnPlatform);
        _signalBus.Subscribe<ChangeGameStateSignal>(OnChangeGameState);
        _camera = Camera.main;
        _minX = 0;
        _maxX = Screen.width;
        Initialize();
    }

    private void OnChangeGameState(ChangeGameStateSignal signal)
    {
        if(signal.ActivatedState == GameState.MainMenuState)
            Initialize();
    }

    private void Initialize()
    {
        _currentPlatform = null;
        _previousPlatform = null;
        for (int i = 0; i < 25; i++) 
            SpawnPlatform();
    }

    private void SpawnPlatform()
    {
        if (_currentPlatform == null)
        {
            _currentPlatform = CreatePlatform(startSpawnPoint.position);
            _currentPlatform.Init();
        }
        else
        {
            _previousPlatform = _currentPlatform;
            
            _currentPlatform = CreatePlatform(Random.Range(0,2) == 0 ? GetPositionPlatformLeft() : GetPositionPlatformFront());
            _previousPlatform.NextPlatform = _currentPlatform;
            if (_camera.WorldToScreenPoint(_currentPlatform.transform.position).x > _maxX)
                _currentPlatform.transform.position = GetPositionPlatformLeft();
            else if (_camera.WorldToScreenPoint(_currentPlatform.transform.position).x < _minX+20)
                _currentPlatform.transform.position = GetPositionPlatformFront();
            _currentPlatform.Init();
        }
        
    }

    private Platform CreatePlatform(Vector3 pos)
    {
        return LeanPool.Spawn(platform, pos, Quaternion.identity);
        
    }

    private Vector3 GetPositionPlatformFront() => 
        _previousPlatform.transform.position + new Vector3(0, 0, _previousPlatform.transform.localScale.z);

    private Vector3 GetPositionPlatformLeft() =>
        _previousPlatform.transform.position - new Vector3(_previousPlatform.transform.localScale.x, 0, 0);

    private void OnDisable()
    {
        _signalBus.TryUnsubscribe<SpawnPlatformSignal>(SpawnPlatform);
        _signalBus.TryUnsubscribe<ChangeGameStateSignal>(OnChangeGameState);
    }
}
