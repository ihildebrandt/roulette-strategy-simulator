namespace RouletteStrategySimulator;

using System.Collections.ObjectModel;

public interface IBet
{
    ReadOnlyCollection<Number> Numbers { get; }

    int Odds { get; }

    bool Resolve(Number number);
}
