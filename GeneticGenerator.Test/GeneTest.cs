namespace GeneticGenerator.Test;

public class GeneTest
{
    [Theory]
    [InlineData(0b000000_000_00, new[] { "1", "2", "3", "4", "5", "6" })] // 1 - 6
    [InlineData(0b000001_000_00, new[] { "4", "5", "6", "7", "8", "9" })] // 4 - 9
    [InlineData(0b000010_000_00, new[] { "7", "8", "9", "10", "11", "12" })] // 7 - 12
    [InlineData(0b000011_000_00, new[] { "10", "11", "12", "13", "14", "15" })] // 10 - 15
    [InlineData(0b000100_000_00, new[] { "13", "14", "15", "16", "17", "18" })] // 13 - 18
    [InlineData(0b000101_000_00, new[] { "16", "17", "18", "19", "20", "21" })] // 16 - 21
    [InlineData(0b000110_000_00, new[] { "19", "20", "21", "22", "23", "24" })] // 19 - 24
    [InlineData(0b000111_000_00, new[] { "22", "23", "24", "25", "26", "27" })] // 22 - 27
    [InlineData(0b001000_000_00, new[] { "25", "26", "27", "28", "29", "30" })] // 25 - 30
    [InlineData(0b001001_000_00, new[] { "28", "29", "30", "31", "32", "33" })] // 28 - 33
    [InlineData(0b001010_000_00, new[] { "31", "32", "33", "34", "35", "36" })] // 31 - 36
    [InlineData(0b001011_000_00, new[] { "1", "2", "3", "4", "5", "6" })] // 1 - 6
    [InlineData(0b001100_000_00, new[] { "4", "5", "6", "7", "8", "9" })] // 4 - 9
    [InlineData(0b001101_000_00, new[] { "7", "8", "9", "10", "11", "12" })] // 7 - 12
    [InlineData(0b001110_000_00, new[] { "10", "11", "12", "13", "14", "15" })] // 10 - 15
    [InlineData(0b001111_000_00, new[] { "13", "14", "15", "16", "17", "18" })] // 13 - 18
    [InlineData(0b010000_000_00, new[] { "16", "17", "18", "19", "20", "21" })] // 16 - 21
    [InlineData(0b010001_000_00, new[] { "19", "20", "21", "22", "23", "24" })] // 19 - 24
    [InlineData(0b010010_000_00, new[] { "22", "23", "24", "25", "26", "27" })] // 22 - 27
    [InlineData(0b010011_000_00, new[] { "25", "26", "27", "28", "29", "30" })] // 25 - 30
    [InlineData(0b010100_000_00, new[] { "28", "29", "30", "31", "32", "33" })] // 28 - 33
    [InlineData(0b010101_000_00, new[] { "31", "32", "33", "34", "35", "36" })] // 31 - 36
    public void GeneCanGenerateAvenueBets(ulong value, string[] names)
    {
        var gene = new Gene(value);
        var bet = gene.ParseBet();
        Assert.Equal(names, bet.Numbers.Select(n => n.Name).ToArray());
    }

    [Theory]
    [InlineData(0b000000_001_00, new[] { "1", "2", "3" })]
    [InlineData(0b001100_001_00, new[] { "1", "2", "3" })]
    public void GeneCanGenerateStreetBet(ulong value, string[] names)
    {
        var gene = new Gene(value);
        var bet = gene.ParseBet();
        Assert.Equal(names, bet.Numbers.Select(n => n.Name).ToArray());
    }

    [Theory]
    [InlineData(0b000000_010_00, new[] { "0", "00", "1", "2", "3" })]
    public void GeneCanGenerateBasketBet(ulong value, string[] names)
    {
        var gene = new Gene(value);
        var bet = gene.ParseBet();
        Assert.Equal(names, bet.Numbers.Select(n => n.Name).ToArray());
    }

    [Theory]
    [InlineData(0b000000_011_00, new[] { "1", "2", "4", "5" })]
    [InlineData(0b000001_011_00, new[] { "2", "3", "5", "6" })]
    [InlineData(0b000010_011_00, new[] { "4", "5", "7", "8" })]
    [InlineData(0b000011_011_00, new[] { "5", "6", "8", "9" })]
    [InlineData(0b010110_011_00, new[] { "1", "2", "4", "5" })]
    [InlineData(0b010111_011_00, new[] { "2", "3", "5", "6" })]
    [InlineData(0b011000_011_00, new[] { "4", "5", "7", "8" })]
    [InlineData(0b011001_011_00, new[] { "5", "6", "8", "9" })]
    public void GeneCanGenerateCornerBet(ulong value, string[] names)
    {
        var gene = new Gene(value);
        var bet = gene.ParseBet();
        Assert.Equal(names, bet.Numbers.Select(n => n.Name).ToArray());
    }

    [Theory]
    [InlineData(0b000000_100_00, new[] { "0", "00" })]
    [InlineData(0b000001_100_00, new[] { "1", "2" })]
    [InlineData(0b000010_100_00, new[] { "2", "3" })]
    [InlineData(0b000011_100_00, new[] { "4", "5" })]
    [InlineData(0b000100_100_00, new[] { "5", "6" })]
    [InlineData(0b011010_100_00, new[] { "1", "4" })]
    [InlineData(0b011011_100_00, new[] { "2", "5" })]
    [InlineData(0b011100_100_00, new[] { "3", "6" })]
    [InlineData(0b111011_100_00, new[] { "0", "00" })]
    public void GeneCanGenerateSplitBet(ulong value, string[] names)
    {
        var gene = new Gene(value);
        var bet = gene.ParseBet();
        Assert.Equal(names, bet.Numbers.Select(n => n.Name).ToArray());
    }

    [Theory]
    [InlineData(0b000000_101_00, new[] { "0" })]
    [InlineData(0b000001_101_00, new[] { "1" })]
    [InlineData(0b000010_101_00, new[] { "2" })]
    [InlineData(0b100101_101_00, new[] { "00" })]
    [InlineData(0b100110_101_00, new[] { "0" })]
    [InlineData(0b100111_101_00, new[] { "1" })]
    public void GeneCanGenerateStraightUpBet(ulong value, string[] names)
    {
        var gene = new Gene(value);
        var bet = gene.ParseBet();
        Assert.Equal(names, bet.Numbers.Select(n => n.Name).ToArray());
    }
}
