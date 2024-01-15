namespace RouletteStrategySimulator;


public static class BetFactory
{
    private static readonly IDictionary<int, IBet> BetCache = new Dictionary<int, IBet>();

    private static IBet? GetCache(int id)
    {
        BetCache.TryGetValue(id, out var bet);
        return bet;
    }

    private static IBet SetCache(int id, IBet bet)
    {
        BetCache[id] = bet;
        return bet;
    }

    private static Func<IBet> CacheRetriever(int id, Func<IBet> builder) =>
        () => GetCache(id) ?? SetCache(id, builder());

    public static IEnumerable<IBet> AllBets()
    {
        yield return Red();
        yield return Black();
        yield return Even();
        yield return Odd();
        yield return High();
        yield return Low();
        yield return Basket();

        foreach (var column in Enumerable.Range(1, 3))
        {
            yield return Column(column);
        }

        foreach (var dozen in Enumerable.Range(0, 3))
        {
            yield return Dozen((dozen * 12) + 1);
        }

        foreach (var avenue in Enumerable.Range(0, 11))
        {
            yield return Avenue((avenue * 3) + 1);
        }

        foreach (var street in Enumerable.Range(0, 12))
        {
            yield return Street((street * 3) + 1);
        }

        foreach (var cornerRow in Enumerable.Range(0, 11))
        {
            foreach (var cornerColumn in Enumerable.Range(1, 2))
            {
                yield return Corner((cornerRow * 3) + cornerColumn);
            }
        }

        yield return SplitRight(0);
        foreach (var splitRightRow in Enumerable.Range(0, 12))
        {
            foreach (var splitRightColumn in Enumerable.Range(1, 2))
            {
                yield return SplitRight((splitRightRow * 3) + splitRightColumn);
            }
        }

        foreach (var splitDown in Enumerable.Range(1, 33))
        {
            yield return SplitDown(splitDown);
        }

        foreach (var straightUp in Enumerable.Range(0, 37))
        {
            yield return StraightUp(straightUp);
        }

        yield return StraightUp(100);
    }

    // Static Bets
    public static IBet Red() =>
        CacheRetriever(1, () => new Bet(1, RouletteBoard.Red))();
    public static IBet Black() =>
        CacheRetriever(2, () => new Bet(1, RouletteBoard.Black))();

    public static IBet Even() =>
        CacheRetriever(3, () => new Bet(1, RouletteBoard.Even))();
    public static IBet Odd() =>
        CacheRetriever(4, () => new Bet(1, RouletteBoard.Odd))();

    public static IBet High() =>
        CacheRetriever(5, () => new Bet(1, RouletteBoard.High))();
    public static IBet Low() =>
        CacheRetriever(6, () => new Bet(1, RouletteBoard.Low))();

    public static IBet Basket() =>
        CacheRetriever(7, () => new Bet(6,
            new[] {
                RouletteBoard.Zero,
                RouletteBoard.DoubleZero,
                RouletteBoard.Numbers[1],
                RouletteBoard.Numbers[2],
                RouletteBoard.Numbers[3],
            }))();


    // Dynamic Bets
    private static int DozenId(int start)
    {
        if (start is not 1 and not 13 and not 25)
        {
            throw new InvalidOperationException();
        }

        return 100 + start;
    }

    public static IBet Dozen(int start) =>
        CacheRetriever(DozenId(start), () => new Bet(2,
            Enumerable.Range(start, 12)
                .Select(n => RouletteBoard.Numbers[n])
                .ToArray()
            ))();

    private static int ColumnId(int start)
    {
        if (start is not 1 and not 2 and not 3)
        {
            throw new InvalidOperationException();
        }

        return 200 + start;
    }

    public static IBet Column(int start) =>
        CacheRetriever(ColumnId(start), () => new Bet(2,
            Enumerable.Range(0, 12)
                .Select(n => RouletteBoard.Numbers[(n * 3) + start])
                .ToArray()
            ))();

    private static int AvenueId(int start)
    {
        if (start == 0 || start > 33 || start % 3 != 1)
        {
            throw new InvalidOperationException();
        }

        return 300 + start;
    }

    public static IBet Avenue(int start) =>
        CacheRetriever(AvenueId(start), () => new Bet(5,
            Enumerable.Range(0, 6)
                .Select(n => RouletteBoard.Numbers[start + n])
                .ToArray()
            ))();

    private static int CornerId(int topLeft)
    {
        if (topLeft == 0 || topLeft % 3 == 0)
        {
            throw new InvalidOperationException();
        }

        return 400 + topLeft;
    }

    public static IBet Corner(int topLeft) =>
        CacheRetriever(CornerId(topLeft), () => new Bet(8,
            Enumerable.Range(0, 4)
                .Select(n => RouletteBoard.Numbers[topLeft + n + (n > 1 ? 1 : 0)])
                .ToArray()
            ))();

    private static int StreetId(int left)
    {
        if (left == 0 || left % 3 != 1)
        {
            throw new NotImplementedException();
        }

        return 500 + left;
    }

    public static IBet Street(int left) =>
        CacheRetriever(StreetId(left), () => new Bet(11,
            Enumerable.Range(0, 3)
                .Select(n => RouletteBoard.Numbers[n + left])
                .ToArray()
            ))();

    private static int SplitRightId(int left)
    {
        if (left != 0 && left % 3 == 0)
        {
            throw new NotImplementedException();
        }

        return 600 + left;
    }

    public static IBet SplitRight(int left) =>
        CacheRetriever(SplitRightId(left), () =>
        {
            var right = left == 0 ? 100 : left + 1;
            var numbers = new[]
            {
                RouletteBoard.Numbers[left],
                RouletteBoard.Numbers[right]
            };

            return new Bet(17, numbers);
        })();

    private static int SplitDownId(int top)
    {
        if (top is 0 or 100 or > 33)
        {
            throw new NotImplementedException();
        }

        return 700 + top;
    }

    public static IBet SplitDown(int top) =>
        CacheRetriever(SplitDownId(top), () => new Bet(18, new[] {
            RouletteBoard.Numbers[top],
            RouletteBoard.Numbers[top + 3]
        }))();

    public static IBet StraightUp(int number) =>
        CacheRetriever(800 + number, () => new Bet(35, new[] { RouletteBoard.Numbers[number] }))();
}
