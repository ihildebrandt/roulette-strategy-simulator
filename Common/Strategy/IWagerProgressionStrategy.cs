namespace RouletteStrategySimulator.Strategy;

internal interface IWagerProgressionStrategy
{
    bool ProgressOnHit { get; }
    bool ProgressOnMiss { get; }
    int GetCurrentMultiple();
}
