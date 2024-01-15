namespace Common.Test;

using RouletteStrategySimulator;

public class BetTest
{
    [Fact]
    public void BlackBetResolvesCorrectly()
    {
        var trueNumbers = new[]
        {
            RouletteBoard.Numbers[2],
            RouletteBoard.Numbers[4],
            RouletteBoard.Numbers[6],
            RouletteBoard.Numbers[8],
            RouletteBoard.Numbers[10],
            RouletteBoard.Numbers[11],
            RouletteBoard.Numbers[13],
            RouletteBoard.Numbers[15],
            RouletteBoard.Numbers[17],
            RouletteBoard.Numbers[20],
            RouletteBoard.Numbers[22],
            RouletteBoard.Numbers[24],
            RouletteBoard.Numbers[26],
            RouletteBoard.Numbers[28],
            RouletteBoard.Numbers[29],
            RouletteBoard.Numbers[31],
            RouletteBoard.Numbers[33],
            RouletteBoard.Numbers[35],
        };

        var falseNumbers = new[]
        {
            RouletteBoard.Zero,
            RouletteBoard.DoubleZero,
            RouletteBoard.Numbers[1],
            RouletteBoard.Numbers[3],
            RouletteBoard.Numbers[5],
            RouletteBoard.Numbers[7],
            RouletteBoard.Numbers[9],
            RouletteBoard.Numbers[12],
            RouletteBoard.Numbers[14],
            RouletteBoard.Numbers[16],
            RouletteBoard.Numbers[18],
            RouletteBoard.Numbers[19],
            RouletteBoard.Numbers[21],
            RouletteBoard.Numbers[23],
            RouletteBoard.Numbers[25],
            RouletteBoard.Numbers[27],
            RouletteBoard.Numbers[30],
            RouletteBoard.Numbers[32],
            RouletteBoard.Numbers[34],
            RouletteBoard.Numbers[36],
        };

        foreach (var number in trueNumbers)
        {
            var bet = BetFactory.Black();
            Assert.True(bet.Resolve(number));
        }

        foreach (var number in falseNumbers)
        {
            var bet = BetFactory.Black();
            Assert.False(bet.Resolve(number));
        }
    }

    [Fact]
    public void FirstColumnBetsResolveCorrectly()
    {
        var falseNumbers = new[] {
            RouletteBoard.Zero,
            RouletteBoard.DoubleZero,
            RouletteBoard.Numbers[2],
            RouletteBoard.Numbers[3],
            RouletteBoard.Numbers[5],
            RouletteBoard.Numbers[6],
            RouletteBoard.Numbers[8],
            RouletteBoard.Numbers[9],
            RouletteBoard.Numbers[11],
            RouletteBoard.Numbers[12],
            RouletteBoard.Numbers[14],
            RouletteBoard.Numbers[15],
            RouletteBoard.Numbers[17],
            RouletteBoard.Numbers[18],
            RouletteBoard.Numbers[20],
            RouletteBoard.Numbers[21],
            RouletteBoard.Numbers[23],
            RouletteBoard.Numbers[24],
            RouletteBoard.Numbers[26],
            RouletteBoard.Numbers[27],
            RouletteBoard.Numbers[29],
            RouletteBoard.Numbers[30],
            RouletteBoard.Numbers[32],
            RouletteBoard.Numbers[33],
            RouletteBoard.Numbers[35],
            RouletteBoard.Numbers[36],
        };

        var trueNumbers = new[]
        {
            RouletteBoard.Numbers[1],
            RouletteBoard.Numbers[4],
            RouletteBoard.Numbers[7],
            RouletteBoard.Numbers[10],
            RouletteBoard.Numbers[13],
            RouletteBoard.Numbers[16],
            RouletteBoard.Numbers[19],
            RouletteBoard.Numbers[22],
            RouletteBoard.Numbers[25],
            RouletteBoard.Numbers[28],
            RouletteBoard.Numbers[31],
            RouletteBoard.Numbers[34],
        };

        foreach (var number in trueNumbers)
        {
            var bet = BetFactory.Column(1);
            Assert.True(bet.Resolve(number));
        }

        foreach (var number in falseNumbers)
        {
            var bet = BetFactory.Column(1);
            Assert.False(bet.Resolve(number));
        }
    }

    [Fact]
    public void RedBetResolvesCorrectly()
    {
        var trueNumbers = new[]
        {
            RouletteBoard.Numbers[1],
            RouletteBoard.Numbers[3],
            RouletteBoard.Numbers[5],
            RouletteBoard.Numbers[7],
            RouletteBoard.Numbers[9],
            RouletteBoard.Numbers[12],
            RouletteBoard.Numbers[14],
            RouletteBoard.Numbers[16],
            RouletteBoard.Numbers[18],
            RouletteBoard.Numbers[19],
            RouletteBoard.Numbers[21],
            RouletteBoard.Numbers[23],
            RouletteBoard.Numbers[25],
            RouletteBoard.Numbers[27],
            RouletteBoard.Numbers[30],
            RouletteBoard.Numbers[32],
            RouletteBoard.Numbers[34],
            RouletteBoard.Numbers[36],
        };

        var falseNumbers = new[]
        {
            RouletteBoard.Zero,
            RouletteBoard.DoubleZero,
            RouletteBoard.Numbers[2],
            RouletteBoard.Numbers[4],
            RouletteBoard.Numbers[6],
            RouletteBoard.Numbers[8],
            RouletteBoard.Numbers[10],
            RouletteBoard.Numbers[11],
            RouletteBoard.Numbers[13],
            RouletteBoard.Numbers[15],
            RouletteBoard.Numbers[17],
            RouletteBoard.Numbers[20],
            RouletteBoard.Numbers[22],
            RouletteBoard.Numbers[24],
            RouletteBoard.Numbers[26],
            RouletteBoard.Numbers[28],
            RouletteBoard.Numbers[29],
            RouletteBoard.Numbers[31],
            RouletteBoard.Numbers[33],
            RouletteBoard.Numbers[35],
        };

        foreach (var number in trueNumbers)
        {
            var bet = BetFactory.Red();
            Assert.True(bet.Resolve(number));
        }

        foreach (var number in falseNumbers)
        {
            var bet = BetFactory.Red();
            Assert.False(bet.Resolve(number));
        }
    }

}
