namespace GeneticGenerator;
using System;

public class Environment : IRandom
{
    private const float MutationChance = 0.2f;

    private readonly Random _random = new();

    public int Next() => _random.Next();

    public int NextCrossOver(int aLength, int bLength)
    {
        var length = Math.Min(aLength, bLength);
        var half = length / 2;
        var minPoint = _random.Next(half);
        var maxPoint = _random.Next(half, length);
        var point = _random.Next(minPoint, maxPoint);
        return point;
    }

    public Gene NextGene()
    {
        var bytes = new byte[sizeof(ulong)];
        _random.NextBytes(bytes);
        return new Gene(BitConverter.ToUInt64(bytes));
    }

    public int NextIndividualGeneCount() => _random.Next(1, 5);

    public MutationType NextMutation()
    {
        var check = _random.NextSingle();
        if (check <= MutationChance)
        {
            var types = Enum.GetValues(typeof(MutationType));
            var type = types.GetValue(_random.Next(types.Length));
            return (MutationType)type!;
        }
        return MutationType.None;
    }
}
