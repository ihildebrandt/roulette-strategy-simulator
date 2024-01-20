namespace RouletteStrategySimulator;

public record Wager(IBet Bet, int Amount)
{
    public Guid Id { get; } = Guid.NewGuid();

    public WagerDisposition Disposition { get; private set; } = WagerDisposition.Unresolved;

    public int Payout { get; private set; }

    public void Resolve(Number number)
    {
        if (Bet.Resolve(number))
        {
            Disposition = WagerDisposition.Won;
            Payout = Amount + (Amount * Bet.Odds);
        }
        else
        {
            Disposition = WagerDisposition.Lost;
            Payout = 0;
        }
    }

    public override string ToString() => $"Wager {Amount} on {Bet}";
}
