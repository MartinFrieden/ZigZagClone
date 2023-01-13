using System;
using DG.Tweening;
using Enums;
using Game.Player;
using Game.Signals;
using InteractionObjects;
using Lean.Pool;
using UnityEngine;
using Zenject;

namespace Game.Platforms
{
    public class Platform : MonoBehaviour
    {
        [SerializeField] private Gem gemPrefab;
        [SerializeField] private Transform gemSpawnPoin;
        [SerializeField] private Rigidbody rigid;
        [SerializeField] private BoxCollider spawnDetectCollider;
        [SerializeField] private SphereCollider autoRunHelperCollider;
        [SerializeField] private Transform middlePoint;
        [Inject] private SignalBus _signalBus;
        public Platform NextPlatform;
        public void Init()
        {
            _signalBus.Subscribe<ChangeGameStateSignal>(OnChangeGameStateSignal);
            autoRunHelperCollider.enabled = PlayerPrefs.GetInt("AutoRun") == 1;
            spawnDetectCollider.enabled = PlayerPrefs.GetInt("AutoRun") == 0;
            SpawnGem();
        }

        private void SpawnGem()
        {
            if (UnityEngine.Random.Range(0, 3) == 1)
            {
                Gem gem = LeanPool.Spawn(gemPrefab, gemSpawnPoin.position, Quaternion.identity);
            }
        }

        public Transform GetNextPoint()
        {
            return NextPlatform.middlePoint;
        }

        public Transform GetCurrentMiddlePoint()
        {
            return middlePoint;
        }

        private void OnDisable()
        {
            _signalBus.TryUnsubscribe<ChangeGameStateSignal>(OnChangeGameStateSignal);
        }

        private void OnChangeGameStateSignal(ChangeGameStateSignal signal)
        {
            autoRunHelperCollider.enabled = PlayerPrefs.GetInt("AutoRun") == 1;
            spawnDetectCollider.enabled = PlayerPrefs.GetInt("AutoRun") == 0;
            if(signal.ExitedState == GameState.GameOverState)
                LeanPool.Despawn(this);
        }

        private void OnTriggerEnter(Collider other)
        {
            _signalBus.Fire<SpawnPlatformSignal>();
        }

        private void OnTriggerExit(Collider other)
        {
            DespawnPlatform();
        }

        private void DespawnPlatform()
        {
            _signalBus.Fire<IncreaseScoreSignal>();
            NextPlatform = null;
            DOVirtual.DelayedCall(0.2f, () => rigid.isKinematic = false);
            DOVirtual.DelayedCall(2, () =>
            {
                rigid.isKinematic = true;
                LeanPool.Despawn(this);
            });
        }
    }
}
