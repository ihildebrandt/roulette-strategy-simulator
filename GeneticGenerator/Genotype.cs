namespace GeneticGenerator;

using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using RouletteStrategySimulator;

internal class Genotype
{
    public ReadOnlyCollection<Gene> Genes { get; private set; }

    public int StepCount => 1;

    public Genotype() => Genes = Array.AsReadOnly(Array.Empty<Gene>());

    public void Read(IEnumerable<Gene> genes) => Genes = Array.AsReadOnly(genes.ToArray());

    public int GetInitialBankRoll()
    {
        return 1000;
    }

    public IEnumerable<Wager> GetWagers(int step) =>
        Genes
            .GroupBy(gene => gene.ParseBet())
            .Select(group => (group.Key, group.Sum(_ => 1)))
            .Select(tuple => new Wager(tuple.Key, tuple.Item2))
            .ToList();

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.AppendLine(CultureInfo.InvariantCulture, $"Step Count: {StepCount}");
        sb.AppendLine(CultureInfo.InvariantCulture, $"--------------");
        for (var step = 0; step < StepCount; step++)
        {
            sb.AppendLine(CultureInfo.InvariantCulture, $"Step: {step + 1}");
            var wagers = GetWagers(step);
            foreach (var wager in wagers)
            {
                sb.AppendLine(CultureInfo.InvariantCulture, $"    {wager}");
            }
        }

        return sb.ToString();
    }
}
