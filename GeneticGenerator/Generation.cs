namespace GeneticGenerator;

using System.Collections.ObjectModel;
using RouletteStrategySimulator;
using RouletteStrategySimulator.Application;

public class Generation : IRunner
{
    private readonly IRandom _random;
    private readonly int _gameRounds;

    public ReadOnlyCollection<Individual> Population { get; }

    public Individual Fittest => Population.OrderByDescending(i => i.Fitness).First();

    public Generation(IRandom random, int populationSize, int gameRounds)
    {
        _random = random;
        _gameRounds = gameRounds;

        Population = Array.AsReadOnly(
            Enumerable.Range(0, populationSize)
                .Select(i =>
                {
                    var geneCount = random.NextIndividualGeneCount();
                    var genes = Enumerable.Range(0, geneCount)
                        .Select(j => random.NextGene())
                        .ToArray();
                    return new Individual(genes);
                })
                .ToArray());
    }

    private Generation(IRandom random, Individual[] population, int gameRounds)
    {
        _random = random;
        _gameRounds = gameRounds;

        Population = Array.AsReadOnly(population);
    }

    public RunResult Run(IGame game)
    {
        // TODO: Parallelize... honestly, this could be a good GPU 
        //       task when I want to spend time learning GPU programming
        for (var round = 0; round < _gameRounds; round++)
        {
            foreach (var individual in Population)
            {
                individual?.Play(game);
            }

            game.Turn();

            foreach (var individual in Population)
            {
                individual?.Resolve();
            }
        }

        // TODO
        return new RunResult();
    }

    public Generation Generate()
    {
        var half = Population.Count / 2;
        var randomPop = Population.OrderBy(_ => _random.Next()).ToArray();
        var popA = randomPop.Take(half).OrderBy(i => i.Fitness).ToArray();
        var popB = randomPop.Skip(half).Take(half).OrderBy(i => i.Fitness).ToArray();

        var population = new List<Individual>();
        for (var i = 0; i < popA.Length; i++)
        {
            var a = popA[i];
            var b = popB[i];

            (a, b) = Breed(a, b);

            population.Add(a);
            population.Add(b);
        }
        return new Generation(_random, population.ToArray(), _gameRounds);
    }

    private (Individual, Individual) Breed(Individual a, Individual b)
    {
        var ma = Mutate(a.Genes);
        var mb = Mutate(b.Genes);

        (var g1, var g2) = CrossOver(ma, mb);

        return (
            new Individual(g1.ToArray()),
            new Individual(g2.ToArray())
        );
    }

    private IList<Gene> Mutate(IList<Gene> oldGenes)
    {
        var newGenes = new List<Gene>();

        for (var i = 0; i < oldGenes.Count; i++)
        {
            switch (_random.NextMutation())
            {
                case MutationType.None:
                default:
                    newGenes.Add(oldGenes[i]);
                    break;
            }
        }

        return newGenes;
    }

    private (IList<Gene>, IList<Gene>) CrossOver(IList<Gene> a, IList<Gene> b)
    {
        var crossOverPoint = _random.NextCrossOver(a.Count, b.Count);

        var a1 = a.Take(crossOverPoint).ToArray();
        var a2 = a.Skip(crossOverPoint).Take(a.Count - crossOverPoint).ToArray();
        var b1 = b.Take(crossOverPoint).ToArray();
        var b2 = b.Skip(crossOverPoint).Take(b.Count - crossOverPoint).ToArray();

        var ca = a1.Concat(b2).ToList();
        var cb = b1.Concat(a2).ToList();

        return (ca, cb);
    }
}
