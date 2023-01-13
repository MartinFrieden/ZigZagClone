using System;
using Enums;
using Game.Platforms;
using Game.Signals;
using UnityEditor.Animations;
using UnityEngine;
using Zenject;

namespace Game.Player
{
    public class PlayerView : MonoBehaviour
    {
        [Range(1,15)]
        [SerializeField] private float speed;
        [SerializeField] private Animator animator;
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private Rigidbody rigidbody;
        [SerializeField] private Transform playerStartPoint;
        [Inject] private SignalBus _signalBus;
        private bool _isAutoRun = false;
        private Vector3 _direction;
        private GameState _gameState;
        private static readonly int DirectionX = Animator.StringToHash("DirectionX");
        private static readonly int State = Animator.StringToHash("GameState");

        void Start()
        {
            _direction = Vector3.forward;
            _signalBus.Subscribe<TouchSignal>(OnTouch);
            _signalBus.Subscribe<ChangeGameStateSignal>(SwitchPlayerState);
        }

        private void OnDisable()
        {
            _signalBus.TryUnsubscribe<TouchSignal>(OnTouch);
            _signalBus.TryUnsubscribe<ChangeGameStateSignal>(SwitchPlayerState);
        }

        private void OnTouch()
        {
            if(_isAutoRun)
                return;
            animator.SetFloat(DirectionX, _direction.x);
            _direction = _direction == Vector3.forward ? Vector3.left : Vector3.forward;
        }

        private void SwitchPlayerState(ChangeGameStateSignal signal)
        {
            switch (signal.ActivatedState)
            {
                case GameState.MainMenuState:
                    animator.SetBool(State, false);
                    _gameState = GameState.MainMenuState;
                    _direction = Vector3.forward;
                    transform.position = playerStartPoint.position;
                    break;
                case GameState.GameState:
                    _isAutoRun = PlayerPrefs.GetInt("AutoRun", 0) != 0;
                    _gameState = GameState.GameState;
                    animator.SetBool(State, true);
                    animator.SetFloat(DirectionX, -1);
                    break;
                case GameState.GameOverState:
                    _gameState = GameState.GameOverState;
                    break;
            }
        }

        private void Update()
        {
            if(_gameState != GameState.GameState)
                return;
            transform.Translate(_direction*Time.deltaTime*speed);
        }

        private void FixedUpdate()
        {
            if(_gameState == GameState.GameOverState || _gameState == GameState.MainMenuState)
                return;
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity, layerMask))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.blue);
            }
            else
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.red);
                _signalBus.Fire(new ChangeGameStateSignal(GameState.GameOverState, GameState.GameState));
                rigidbody.AddForce(_direction*2, ForceMode.Impulse);
            }
        }

        
        //AutoRun
        private void OnTriggerEnter(Collider other)
        {
            if(!_isAutoRun)
                return;
            if (other.CompareTag("MiddlePlatformPoint"))
            {
                Platform currentPlatform = other.GetComponentInParent<Platform>();
                _direction = (currentPlatform.GetNextPoint().position - currentPlatform.GetCurrentMiddlePoint().position).normalized;
                animator.SetFloat(DirectionX, _direction.x == -1 ? 0 : -1);

            }
        }
    }
}
