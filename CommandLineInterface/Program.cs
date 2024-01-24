
using GeneticGenerator;
using RouletteStrategySimulator;

var generationNumber = 1;
var populationSize = 50;

var game = new RouletteGame(RouletteBoard.Wheel);
var generation = new Generation(new GeneticGenerator.Environment(), populationSize, 100);

var maxFitness = 0.0f;

while (true)
{
    generation.Run(game);

    var fittest = generation.Fittest;
    if (fittest.Fitness > maxFitness)
    {
        maxFitness = fittest.Fitness;
        Console.SetCursorPosition(0, Console.CursorTop);
        Console.WriteLine($"Generation {generationNumber} ({fittest.Fitness})");
        Console.WriteLine(fittest);
        Console.WriteLine();
    }
    else
    {
        Console.SetCursorPosition(0, Console.CursorTop);
        Console.Write($"Generation {generationNumber}");
    }

    generation = generation.Generate();

    // Debugger.Break();
    generationNumber++;
}
