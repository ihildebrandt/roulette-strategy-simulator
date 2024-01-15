namespace RouletteStrategySimulator.Strategy;

using System.Collections.Generic;

public class Fibonacci : IStrategy
{
    private int _last, _next = 1;

    private readonly int _wagerBasis;
    private readonly int _initialBankRoll;
    private readonly int _retirementDelta;

    public int BankRoll { get; private set; }

    public StrategyDisposition Disposition { get; private set; } = StrategyDisposition.Active;

    public Fibonacci(int bankRoll = 4000, int wagerBasis = 100, int retirementDelta = 1000)
    {
        _wagerBasis = wagerBasis;
        _initialBankRoll = bankRoll;
        _retirementDelta = retirementDelta;
        BankRoll = _initialBankRoll;
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
        var amount = _wagerBasis * _next;

        BankRoll -= amount;
        return new[]
        {
            new Wager(BetFactory.Column(3), amount)
        };
    }

    public void ProcessResult(WagerResult result)
    {
        var wager = result.Wagers.Single();
        if (wager.Disposition == WagerDisposition.Won)
        {
            _last = 0;
            _next = 1;
            BankRoll += wager.Payout;
        }
        else
        {
            var fib = _last + _next;
            _last = _next;
            _next = fib;
        }

        if (BankRoll < _next * _wagerBasis)
        {
            Disposition = StrategyDisposition.Bust;
        }

        if (BankRoll >= _initialBankRoll + _retirementDelta)
        {
            Disposition = StrategyDisposition.Retired;
        }
    }
}
