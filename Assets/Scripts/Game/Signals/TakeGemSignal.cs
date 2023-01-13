namespace Game.Signals
{
    public struct TakeGemSignal
    {
        public readonly int Amount;

        public TakeGemSignal(int amount)
        {
            Amount = amount;
        }
    }
}