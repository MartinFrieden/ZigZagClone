using Enums;

namespace Game.Signals
{
    public readonly struct ChangeGameStateSignal
    {
        public readonly GameState ActivatedState;
        public readonly GameState ExitedState;
        public ChangeGameStateSignal(GameState activatedState, GameState exitState)
        {
            ActivatedState = activatedState;
            ExitedState = exitState;
        }
    }
}