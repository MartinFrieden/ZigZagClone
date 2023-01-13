using Game;
using Game.Input;
using OSSC;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class ServicesInstaller : MonoInstaller
    {
        [SerializeField] private SoundController soundController;
        public override void InstallBindings()
        {
            Container.Bind<SoundController>().FromInstance(soundController).AsSingle();
            Container.BindInterfacesAndSelfTo<TouchInput>().AsSingle();
            Container.BindInterfacesAndSelfTo<AutoInput>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameProgress>().AsSingle();
        }
    }
}
