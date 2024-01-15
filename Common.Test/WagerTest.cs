namespace Common.Test;

using Moq;
using RouletteStrategySimulator;

public class WagerTest
{
    [Theory]
    [InlineData(1, 100, 200)]
    [InlineData(2, 100, 300)]
    [InlineData(3, 100, 400)]
    [InlineData(5, 100, 600)]
    [InlineData(7, 100, 800)]
    [InlineData(11, 100, 1200)]
    [InlineData(25, 100, 2600)]
    public void ResolveWinningWager(int odds, int bet, int payout)
    {
        var winningBetMock = new Mock<IBet>();
        winningBetMock.Setup(m => m.Odds).Returns(odds);
        winningBetMock.Setup(m => m.Resolve(It.IsAny<Number>())).Returns(true);

        var wager = new Wager(winningBetMock.Object, bet);
        Assert.Equal(WagerDisposition.Unresolved, wager.Disposition);

        wager.Resolve(new Number());
        Assert.Equal(WagerDisposition.Won, wager.Disposition);
        Assert.Equal(payout, wager.Payout);
    }
}
