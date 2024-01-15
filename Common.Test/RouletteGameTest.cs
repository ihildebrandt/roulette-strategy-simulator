namespace Common.Test;

using Moq;
using RouletteStrategySimulator;

public class RouletteGameTest
{
    [Fact]
    public void TurnResolvesAllWagers()
    {
        var number = new Number(Color.Green, "00");

        var wheel = new Mock<IWheel>();
        wheel.Setup(m => m.Spin()).Returns(number);

        var game = new RouletteGame(wheel.Object);
        var round = game.PlaceBets(new[]
        {
            new Wager(new Bet(1, new[] { number }), 100),
            new Wager(new Bet(1, new[] { number }), 100),
            new Wager(new Bet(1, new[] { number }), 100)
        });
        game.Turn();

        Assert.All(round.Result.Wagers, w => Assert.Equal(WagerDisposition.Won, w.Disposition));
    }
}
