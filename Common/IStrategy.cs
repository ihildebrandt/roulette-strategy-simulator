namespace RouletteStrategySimulator;
public interface IStrategy
{
    StrategyDisposition Disposition { get; }

    int BankRoll { get; }

    bool ShouldPlay();

    void Play(IGame game);
}
