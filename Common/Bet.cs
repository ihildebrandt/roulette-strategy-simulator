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

    public override int GetHashCode() => ToString().GetHashCode();

    public override bool Equals(object? obj)
    {
        if (obj is not IBet other)
        {
            return false;
        }

        var tn = ToString();
        var on = other.ToString();

        return tn.Equals(on, StringComparison.Ordinal);
    }

    public override string ToString() => string.Join(' ', Numbers.OrderBy(n => n.Name).Select(n => n.Name));
}
