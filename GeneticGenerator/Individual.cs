namespace GeneticGenerator;

using System.Collections.ObjectModel;
using System.Text;
using RouletteStrategySimulator;

public class Individual : IStrategy
{
    public float Fitness => CalculateFitness();

    public StrategyDisposition Disposition { get; } = StrategyDisposition.Active;

    public int BankRoll { get; private set; }

    public ReadOnlyCollection<Gene> Genes => _genotype.Genes;

    private Wager[]? _nextRound;
    private Task<WagerResult>? _resultTask;
    private readonly Genotype _genotype;


    public Individual(IEnumerable<Gene> genes)
    {
        _genotype = new Genotype(genes);
        BankRoll = _genotype.GetInitialBankRoll();
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

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.AppendLine(_genotype.ToString());
        return sb.ToString();
    }

    private void BuildRound()
    {
        var requiredBankroll = 0;
        var wagers = _genotype.GetWagers(1);
        requiredBankroll = wagers.Sum(w => w.Amount);

        // TODO: make exit strategy a part of the gene
        if (BankRoll >= requiredBankroll)
        {
            _nextRound = wagers.ToArray();
            BankRoll -= requiredBankroll;
        }
    }

    private void BuildRound(WagerResult result)
    {
        // TODO: Clean this up
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
        var steps = _genotype.StepCount;
        var totalFitness = 0.0f;

        for (var step = 0; step < steps; step++)
        {
            var wagers = _genotype.GetWagers(step);
            totalFitness += CalculateFitness(wagers);
        }

        return totalFitness / steps;
    }

    private static float CalculateFitness(IEnumerable<Wager> wagers)
    {
        // Static Fitness Calculation
        var fitness = 0.0f;

        // TODO: weighted fitness scores
        fitness += CalculateCoverageFitness(wagers);
        fitness += CalculateMultipleCoverageFitness(wagers);
        fitness += CalculateTotalBetHighPayoutRatio(wagers);
        fitness += CalculateTotalBetLowPayoutRatio(wagers);

        // TODO: Determine how to baseline
        // fitness += CalculateRoundsToBust(wagers);
        // fitness += CalculateRoundsToRetire(wagers);

        // Dynamic Fitness Calculation
        // TODO: Calculate fitness based on outcome of game
        //       - Disposition
        //       - Profit
        //       - Number of rounds survived

        return fitness;
    }

    private static float CalculateCoverageFitness(IEnumerable<Wager> wagers)
    {
        var numerator = wagers.SelectMany(b => b.Bet.Numbers).Distinct().Count();
        var denominator = RouletteBoard.Numbers.Count;

        return (float)numerator / denominator;
    }

    private static float CalculateMultipleCoverageFitness(IEnumerable<Wager> wagers)
    {
        var numerator = wagers.SelectMany(b => b.Bet.Numbers).GroupBy(n => n).Where(g => g.Count() > 1).Select(g => g.Key).Count();
        var denominator = RouletteBoard.Numbers.Count;

        return (float)numerator / denominator;
    }

    private static float CalculateTotalBetHighPayoutRatio(IEnumerable<Wager> wagers)
    {
        var maxOdds = BetFactory.AllBets().Max(i => i.Odds);
        (var highAmount, var highOdds) = wagers.Select(w => (w.Amount, w.Bet.Odds))
                                               .OrderByDescending(i => i.Amount + (i.Amount * i.Odds))
                                               .First();
        var betTotal = wagers.Select(b => b.Amount).Sum();

        var highPayout = highAmount + (highAmount * highOdds);
        var maxPayout = highAmount + (highAmount * maxOdds);

        if (highPayout <= betTotal)
        {
            return 0.0f;
        }

        var numerator = highPayout - betTotal;
        var denominator = maxPayout - betTotal;
        return (float)numerator / denominator;
    }

    private static float CalculateTotalBetLowPayoutRatio(IEnumerable<Wager> wagers)
    {
        var minOdds = 1;
        (var lowAmount, var lowOdds) = wagers.Select(w => (w.Amount, w.Bet.Odds))
                                             .OrderBy(i => i.Amount + (i.Amount * i.Odds))
                                             .First();

        var betTotal = wagers.Select(b => b.Amount).Sum();
        var lowPayout = lowAmount + (lowAmount * lowOdds);
        var minPayout = lowAmount + (lowAmount * minOdds);

        if (lowPayout >= betTotal)
        {
            return 1.0f;
        }

        var numerator = lowPayout - minPayout;
        var denominator = betTotal - minPayout;

        return (float)numerator / denominator;
    }
}
