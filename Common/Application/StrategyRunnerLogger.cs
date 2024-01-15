namespace RouletteStrategySimulator.Application;
public class StrategyRunnerLogger : IRunner
{
    private readonly IRunner _innerRunner;

    public StrategyRunnerLogger(IRunner innerRunner) =>
        _innerRunner = innerRunner;

    public RunResult Run(IGame game)
    {
        var result = _innerRunner.Run(game);
        Console.WriteLine(result);
        return result;
    }
}
