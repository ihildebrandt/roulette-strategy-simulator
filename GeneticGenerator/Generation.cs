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
                if (individual.ShouldPlay())
                {
                    individual.Play(game);
                }
            }

            game.Turn();

            foreach (var individual in Population)
            {
                individual.Resolve();
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
            var pa = popA[i];
            var pb = popB[i];

            (var ca, var cb) = Breed(pa, pb);

            population.Add(ca);
            population.Add(cb);
        }
        return new Generation(_random, population.ToArray(), _gameRounds);
    }

    private (Individual, Individual) Breed(Individual a, Individual b)
    {
        var aBytes = a.Genes.SelectMany(g => BitConverter.GetBytes(g.Value)).ToList();
        var bBytes = a.Genes.SelectMany(g => BitConverter.GetBytes(g.Value)).ToList();

        var aBytesMut = Mutate(aBytes);
        var bBytesMut = Mutate(bBytes);

        (var c1Bytes, var c2Bytes) = CrossOver(aBytesMut, bBytesMut);

        static IEnumerable<Gene> convertToGenes(byte[] bytes)
        {
            var size = sizeof(ulong);
            var genes = new List<Gene>();

            var i = 0;

            for (i = 0; i < bytes.Length && bytes.Length - i >= size; i += size)
            {
                var geneBytes = new byte[size];
                Array.Copy(bytes, i, geneBytes, 0, size);
                genes.Add(new Gene(BitConverter.ToUInt64(geneBytes)));
            }

            if (i < bytes.Length)
            {
                var geneBytes = new byte[size];
                Array.Copy(bytes, i, geneBytes, 0, bytes.Length - i);
                genes.Add(new Gene(BitConverter.ToUInt64(geneBytes)));
            }

            return genes;
        }

        return (
            new Individual(convertToGenes(c1Bytes.ToArray())),
            new Individual(convertToGenes(c2Bytes.ToArray()))
        );
    }

    private IList<byte> Mutate(IList<byte> oldBytes)
    {
        var newBytes = new List<byte>();

        for (var i = 0; i < oldBytes.Count; i++)
        {
            switch (_random.NextMutation())
            {
                case MutationType.Random:
                    newBytes.Add((byte)_random.Next());
                    break;
                case MutationType.Duplicate:
                    newBytes.Add(oldBytes[i]);
                    newBytes.Add(oldBytes[i]);
                    break;
                case MutationType.Transpose:
                    if (i < oldBytes.Count - 1)
                    {
                        newBytes.Add(oldBytes[i + 1]);
                    }
                    newBytes.Add(oldBytes[i]);
                    i++;
                    break;
                case MutationType.Delete:
                    break;
                case MutationType.Insert:
                    newBytes.Add(oldBytes[i]);
                    newBytes.Add((byte)_random.Next());
                    break;
                case MutationType.None:
                default:
                    newBytes.Add(oldBytes[i]);
                    break;
            }
        }

        return newBytes;
    }

    private (IList<byte>, IList<byte>) CrossOver(IList<byte> a, IList<byte> b)
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
