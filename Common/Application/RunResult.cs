namespace RouletteStrategySimulator.Application;

public readonly record struct RunResult(StrategyDisposition Disposition, int StartingBankRoll, int EndingBankRoll, int Turns);
