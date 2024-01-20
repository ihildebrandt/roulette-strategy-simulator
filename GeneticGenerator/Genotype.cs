namespace GeneticGenerator;

using System.Collections.ObjectModel;
using RouletteStrategySimulator;

internal class Genotype
{
    public ReadOnlyCollection<Gene> Genes => Array.AsReadOnly(_genes);

    private readonly Gene[] _genes;

    public int StepCount => 1;

    public Genotype(IEnumerable<Gene> genes)
    {
        _genes = genes.ToArray();
    }

    public int GetInitialBankRoll()
    {
        return 1000;
    }

    public IEnumerable<Wager> GetWagers(int step)
    {
        // TODO: Make wager amount part of gene
        return _genes.Select(g => new Wager(g.ParseBet(), 1)).ToArray();
    }
}
