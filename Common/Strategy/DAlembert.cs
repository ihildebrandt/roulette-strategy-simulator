namespace RouletteStrategySimulator.Strategy;

using System.Collections.Generic;

public class DAlembert : IStrategy
{
    public static DAlembert AlwaysBetOnRed(int initialBankRoll, int wagerBasis, int retirementDelta) =>
        new(initialBankRoll, wagerBasis, retirementDelta, () => BetFactory.Red());

    private readonly int _initialBankRoll;

    private readonly int _wagerBasis;

    private readonly int _retirementDelta;

    private readonly Func<IBet> _betGenerator;
    
    private int betAmount;

    public StrategyDisposition Disposition { get; private set; } = StrategyDisposition.Active;

    public int BankRoll { get; private set; }

    public DAlembert(int initialBankRoll, int wagerBasis, int retirementDelta, Func<IBet> betGenerator)
    {
        _initialBankRoll = initialBankRoll;
        _wagerBasis = wagerBasis;
        _retirementDelta = retirementDelta;

        betAmount = wagerBasis * 5;
        _betGenerator = betGenerator;
        BankRoll = initialBankRoll;
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
        BankRoll -= betAmount;
        return new[]
        {
            new Wager(_betGenerator(), betAmount)
        };
    }

    public void ProcessResult(WagerResult result)
    {
        var wager = result.Wagers.First();
        BankRoll += wager.Payout;

        if (wager.Disposition == WagerDisposition.Won)
        {
            betAmount -= _wagerBasis;
        }
        else
        {
            betAmount += _wagerBasis;
        }

        if (BankRoll < betAmount)
        {
            Disposition = StrategyDisposition.Bust;
        }

        if (BankRoll >= _initialBankRoll + _retirementDelta)
        {
            Disposition = StrategyDisposition.Retired;
        }
    }
}
