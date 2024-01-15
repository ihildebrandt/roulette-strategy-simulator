namespace RouletteStrategySimulator;

public static class RouletteBoard
{
    public static readonly IDictionary<int, Number> Numbers = new Dictionary<int, Number>
    {
        { 0, new Number(Color.Green, "0") },
        { 100, new Number(Color.Green, "00") },
        { 1, new Number(Color.Red, "1") },
        { 2, new Number(Color.Black, "2") },
        { 3, new Number(Color.Red, "3") },
        { 4, new Number(Color.Black, "4") },
        { 5, new Number(Color.Red, "5") },
        { 6, new Number(Color.Black, "6") },
        { 7, new Number(Color.Red, "7") },
        { 8, new Number(Color.Black, "8") },
        { 9, new Number(Color.Red, "9") },
        { 10, new Number(Color.Black, "10") },
        { 11, new Number(Color.Black, "11") },
        { 12, new Number(Color.Red, "12") },
        { 13, new Number(Color.Black, "13") },
        { 14, new Number(Color.Red, "14") },
        { 15, new Number(Color.Black, "15") },
        { 16, new Number(Color.Red, "16") },
        { 17, new Number(Color.Black, "17") },
        { 18, new Number(Color.Red, "18") },
        { 19, new Number(Color.Red, "19") },
        { 20, new Number(Color.Black, "20") },
        { 21, new Number(Color.Red, "21") },
        { 22, new Number(Color.Black, "22") },
        { 23, new Number(Color.Red, "23") },
        { 24, new Number(Color.Black, "24") },
        { 25, new Number(Color.Red, "25") },
        { 26, new Number(Color.Black, "26") },
        { 27, new Number(Color.Red, "27") },
        { 28, new Number(Color.Black, "28") },
        { 29, new Number(Color.Black, "29") },
        { 30, new Number(Color.Red, "30") },
        { 31, new Number(Color.Black, "31") },
        { 32, new Number(Color.Red, "32") },
        { 33, new Number(Color.Black, "33") },
        { 34, new Number(Color.Red, "34") },
        { 35, new Number(Color.Black, "35") },
        { 36, new Number(Color.Red, "36") },
    };

    public static readonly Number Zero;
    public static readonly Number DoubleZero;

    public static Number[] Red => Numbers.Where(n => n.Value.Color == Color.Red).Select(n => n.Value).ToArray();
    public static Number[] Black => Numbers.Where(n => n.Value.Color == Color.Black).Select(n => n.Value).ToArray();
    public static Number[] Even => Numbers.Where(n => n.Key > 0 && n.Key < 100 && n.Key % 2 == 0).Select(n => n.Value).ToArray();
    public static Number[] Odd => Numbers.Where(n => n.Key > 0 && n.Key < 100 && n.Key % 2 == 1).Select(n => n.Value).ToArray();
    public static Number[] Low => Numbers.Where(n => n.Key is > 0 and <= 18).Select(n => n.Value).ToArray();
    public static Number[] High => Numbers.Where(n => n.Key is > 18 and <= 36).Select(n => n.Value).ToArray();

    public static IWheel Wheel { get; } = new RouletteWheel();

    static RouletteBoard()
    {
        Zero = Numbers[0];
        DoubleZero = Numbers[100];
    }

    public class RouletteWheel : IWheel
    {
        private readonly Random _random;

        public RouletteWheel() => _random = new Random();

        public Number Spin()
        {
            var keys = Numbers.Keys.ToArray();
            var key = keys[_random.Next(keys.Length)];
            return Numbers[key];
        }
    }
}
