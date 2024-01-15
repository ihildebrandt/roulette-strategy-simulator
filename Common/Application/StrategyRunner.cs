namespace RouletteStrategySimulator.Application;
public class StrategyRunner : IRunner
{
    private readonly IStrategy _strategy;

    public StrategyRunner(IStrategy strategy) =>
        _strategy = strategy;

    public RunResult Run(IGame game)
    {
        var startingBankroll = _strategy.BankRoll;
        var turns = 0;

        while (_strategy.ShouldPlay())
        {
            _strategy.Play(game);
        }

        return new RunResult(
            _strategy.Disposition,
            startingBankroll,
            _strategy.BankRoll,
            turns);
    }
}
