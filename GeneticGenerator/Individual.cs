namespace GeneticGenerator;

using System.Collections.ObjectModel;
using RouletteStrategySimulator;

public class Individual : IStrategy
{
    public float Fitness => CalculateFitness();

    public ReadOnlyCollection<Gene> Genes { get; }

    public StrategyDisposition Disposition { get; } = StrategyDisposition.Active;

    public int BankRoll { get; private set; }

    private Wager[]? _nextRound;
    private Task<WagerResult>? _resultTask;
    private readonly int _initialBankRoll = 1000;

    public Individual(Gene[] genes)
    {
        BankRoll = _initialBankRoll;
        Genes = Array.AsReadOnly(genes);
        BuildRound();
    }

    public bool ShouldPlay() => _nextRound?.Length > 0;

    public void Play(IGame game)
    {
        if (_nextRound == null)
        {
            throw new InvalidOperationException();
        }

        _resultTask = game.PlaceBets(_nextRound);
    }

    public void Resolve()
    {
        if (_resultTask == null)
        {
            throw new InvalidOperationException();
        }

        var result = _resultTask.Result;
        BuildRound(result);
    }

    private void BuildRound()
    {
        var requiredBankroll = 0;
        var bets = Genes.Select(g => g.ParseBet());
        var nextRound = new List<Wager>();

        foreach (var bet in bets)
        {
            var wager = new Wager(bet, 1);
            nextRound.Add(wager);
            requiredBankroll++; // TODO: make wager amount a part of the gene
        }

        // TODO: make exit strategy a part of the gene
        if (BankRoll >= requiredBankroll)
        {
            _nextRound = nextRound.ToArray();
            BankRoll -= requiredBankroll;
        }
    }

    private void BuildRound(WagerResult result)
    {
        foreach (var wager in result.Wagers)
        {
            BankRoll += wager.Payout;
            if (wager.Bet is Gene.Bet bet)
            {
                bet.Gene.Step(wager.Disposition);
            }
        }

        BuildRound();
    }

    private float CalculateFitness()
    {
        // Static Fitness Calculation
        var fitness = 0.0f;
        fitness += CalculateCoverageFitness();
        fitness += CalculateMultipleCoverageFitness();

        // TODO: Calculate fitness based on bet / payout ratio
        //       -

        // TODO: Calculate fitness based on number of rounds to bust

        // TODO: Calculate fitness based on number of rounds to retire

        // Dynamic Fitness Calculation
        // TODO: Calculate fitness based on outcome of game
        //       - Disposition
        //       - Profit
        //       - Number of rounds survived

        return fitness;
    }

    private float CalculateCoverageFitness()
    {
        var bets = Genes.Select(g => g.ParseBet()).ToArray();
        var numerator = bets.SelectMany(b => b.Numbers).Distinct().Count();
        var denominator = RouletteBoard.Numbers.Count;

        return (float)numerator / denominator;
    }

    private float CalculateMultipleCoverageFitness()
    {
        var bets = Genes.Select(g => g.ParseBet()).ToArray();
        var numerator = bets.SelectMany(b => b.Numbers).GroupBy(n => n).Where(g => g.Count() > 1).Select(g => g.Key).Count();
        var denominator = RouletteBoard.Numbers.Count;

        return (float)numerator / denominator;
    }
}
