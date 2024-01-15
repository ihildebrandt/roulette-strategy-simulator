namespace RouletteStrategySimulator.Strategy;

using System.Collections.Generic;

public class DoubleStreets : IStrategy
{
    private readonly IBetCalculator _betCalculator;
    private readonly int _initialBankRoll;

    private int _winStreak;

    public StrategyDisposition Disposition { get; private set; } = StrategyDisposition.Active;

    public int BankRoll { get; private set; }

    public DoubleStreets(int bankRoll, IBetCalculator betCalculator)
    {
        _initialBankRoll = bankRoll;
        _betCalculator = betCalculator;

        BankRoll = bankRoll;
    }

    public bool ShouldPlay() => Disposition == StrategyDisposition.Active;

    public void Play(IGame game)
    {
        var wagers = GenerateWagers();
        var round = game.PlaceBets(wagers);
        game.Turn();
        ProcessResult(round.Result);
    }

    public IEnumerable<Wager> GenerateWagers()
    {
        yield return new Wager(BetFactory.Avenue(4), 10);
        yield return new Wager(BetFactory.Avenue(10), 10);
        yield return new Wager(BetFactory.Avenue(16), 10);
        yield return new Wager(BetFactory.Avenue(22), 10);
        yield return new Wager(BetFactory.Avenue(28), 10);
    }

    public void ProcessResult(WagerResult result)
    {
        var wager = result.Wagers.Single(w => w.Disposition == WagerDisposition.Won);

        if (wager != null)
        {
            BankRoll += wager.Payout;
        }
        else
        {

        }
    }
}
