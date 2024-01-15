namespace RouletteStrategySimulator.Strategy;

public interface IBetCalculator
{
    int GetBasis(int currentBankRoll, int numberOfBets);
    int GetMultiplier(int lossCount);
}
