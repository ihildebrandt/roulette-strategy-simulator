namespace RouletteStrategySimulator.Application;
public class StatsRunner : IRunner
{
    private readonly Func<IRunner> _innerRunnerFactory;

    private readonly int _numberOfGames;
    public StatsRunner(Func<IRunner> innerRunnerFactory, int numberOfGames)
    {
        _innerRunnerFactory = innerRunnerFactory;
        _numberOfGames = numberOfGames;
    }

    public RunResult Run(IGame game)
    {
        int won = 0, total = 0;
        int startingBankRoll = 0, endingBankRoll = 0;

        while (total < _numberOfGames)
        {
            var runner = _innerRunnerFactory();
            var result = runner.Run(game);

            startingBankRoll += result.StartingBankRoll;
            endingBankRoll += result.EndingBankRoll;

            if (result.Disposition == StrategyDisposition.Retired)
            {
                won++;
            }

            total++;
        }

        Console.WriteLine($"" +
            $"Won: {won}\n" +
            $"Total: {total}\n" +
            $"Pct: {won / (float)total}\n" +
            $"Total Starting BankRoll: ${startingBankRoll}\n" +
            $"Total Ending BankRoll: ${endingBankRoll}\n" +
            $"Average Change: {endingBankRoll / (float)startingBankRoll}");

        return new RunResult();
    }
}
