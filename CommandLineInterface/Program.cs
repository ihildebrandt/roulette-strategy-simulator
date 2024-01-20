
using GeneticGenerator;
using RouletteStrategySimulator;

var generationNumber = 1;
var populationSize = 50;

var game = new RouletteGame(RouletteBoard.Wheel);
var generation = new Generation(new GeneticGenerator.Environment(), populationSize, 100);

var maxFitness = 0.0f;

while (true)
{
    Console.SetCursorPosition(0, 0);
    Console.WriteLine($"Generation {generationNumber}");

    generation.Run(game);

    var fittest = generation.Fittest;
    if (fittest.Fitness > maxFitness)
    {
        Console.Clear();
        Console.SetCursorPosition(0, 0);
        Console.WriteLine($"Generation {generationNumber}");

        maxFitness = fittest.Fitness;
        Console.WriteLine($"Generation {generationNumber} ({fittest.Fitness})");
        Console.WriteLine(fittest);

        /*
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
        */
    }

    generation = generation.Generate();

    // Debugger.Break();
    generationNumber++;
}
