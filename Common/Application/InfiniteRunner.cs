namespace RouletteStrategySimulator.Application;
public class InfiniteRunner : IRunner
{
    private readonly Func<IRunner> _runnerFactory;

    public InfiniteRunner(Func<IRunner> runnerFactory) => _runnerFactory = runnerFactory;

    public RunResult Run(IGame game)
    {
        while (true)
        {
            var runner = _runnerFactory();
            var result = runner.Run(game);
            Console.WriteLine(result);
        }
    }
}
