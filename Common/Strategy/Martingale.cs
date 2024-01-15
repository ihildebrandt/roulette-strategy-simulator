namespace RouletteStrategySimulator.Strategy;

using System.Collections.Generic;

public class Martingale : IStrategy
{
    public static Martingale AlwaysBetOnRed(int initialBankRoll, int wagerBasis, int retirementDelta) =>
        new(initialBankRoll, wagerBasis, retirementDelta, () => BetFactory.Red());

    public static Martingale AlwaysBetOnBlack(int initialBankRoll, int wagerBasis, int retirementDelta) =>
        new(initialBankRoll, wagerBasis, retirementDelta, () => BetFactory.Black());

    private readonly int _initialBankRoll;
    private readonly int _initialBetAmount;
    private readonly int _retirementDelta;

    private readonly Func<IBet> _betSelector;

    private int _nextBetAmount;

    public StrategyDisposition Disposition { get; private set; } = StrategyDisposition.Active;

    public int BankRoll { get; private set; }

    public Martingale(int initialBankRoll, int wagerBasis, int retirementDelta, Func<IBet> betSelector)
    {
        _initialBankRoll = initialBankRoll;
        _initialBetAmount = wagerBasis;
        _retirementDelta = retirementDelta;
        _betSelector = betSelector;

        _nextBetAmount = _initialBetAmount;
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
        BankRoll -= _nextBetAmount;
        return new[]
        {
            new Wager(_betSelector(), _nextBetAmount)
        };
    }

    public void ProcessResult(WagerResult result)
    {
        var wager = result.Wagers.First();
        BankRoll += wager.Payout;
        if (wager.Disposition == WagerDisposition.Won)
        {
            _nextBetAmount = _initialBetAmount;
        }
        else
        {
            _nextBetAmount *= 2;
        }

        if (BankRoll < _nextBetAmount)
        {
            Disposition = StrategyDisposition.Bust;
        }

        if (BankRoll >= _initialBankRoll + _retirementDelta)
        {
            Disposition = StrategyDisposition.Retired;
        }
    }
}
