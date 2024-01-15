
using System.Diagnostics;
using GeneticGenerator;
using RouletteStrategySimulator;

var generationNumber = 1;
var populationSize = 50;

var game = new RouletteGame(RouletteBoard.Wheel);
var generation = new Generation(new GeneticGenerator.Environment(), populationSize, 100);

while (true)
{
    generation.Run(game);

    var fittest = generation.Fittest;
    Console.WriteLine($"Generation {generationNumber}");

    // TODO: Move to some logging class 
    foreach (var gene in fittest.Genes)
    {
        var bet = gene.ParseBet();
        Console.Write($"    ");
        foreach (var number in bet.Numbers)
        {
            Console.Write($"{number.Name} ");
        }
        Console.WriteLine();
    }

    generation.Generate();

    Debugger.Break();
    generationNumber++;
}
