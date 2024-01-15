namespace RouletteStrategySimulator;

using System.Collections.ObjectModel;

internal class Bet : IBet
{
    public ReadOnlyCollection<Number> Numbers { get; }

    public int Odds { get; }

    public bool Resolve(Number number) =>
        Numbers.Contains(number);

    public Bet(int odds, Number[] winningNumbers)
    {
        Odds = odds;
        Numbers = Array.AsReadOnly(winningNumbers);
    }
}
