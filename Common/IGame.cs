namespace RouletteStrategySimulator;

public interface IGame
{
    Task<WagerResult> PlaceBets(IEnumerable<Wager> wagers);
    void Turn();
}
