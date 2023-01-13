using Game.Signals;
using Zenject;

namespace Installers
{
    public class SignalInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
            ConfigureDefaultSettings();
            Container.DeclareSignal<TouchSignal>();
            Container.DeclareSignal<ChangeGameStateSignal>();
            Container.DeclareSignal<SpawnPlatformSignal>();
            Container.DeclareSignal<TakeGemSignal>();
            Container.DeclareSignal<IncreaseScoreSignal>();
        }
    
        private void ConfigureDefaultSettings()
        {
            var signalSettings = new ZenjectSettings.SignalSettings(SignalDefaultSyncModes.Synchronous,
                SignalMissingHandlerResponses.Ignore);
            var settings = new ZenjectSettings(ValidationErrorResponses.Throw, signalSettings: signalSettings);
            Container.Settings = settings;
        }
    }
}
