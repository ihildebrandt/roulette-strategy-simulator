namespace GeneticGenerator;

using System.Collections.ObjectModel;
using RouletteStrategySimulator;

public record Gene(ulong Value)
{
    internal class Bet : IBet
    {
        private readonly IBet _innerBet;

        // HACK: This feels like a smell
        public Gene Gene { get; }

        public ReadOnlyCollection<Number> Numbers => _innerBet.Numbers;

        public int Odds => _innerBet.Odds;

        public Bet(IBet innerBet, Gene gene)
        {
            _innerBet = innerBet;
            Gene = gene;
        }

        public bool Resolve(Number number) => _innerBet.Resolve(number);

        public override int GetHashCode() => _innerBet.GetHashCode();

        public override bool Equals(object? obj) => _innerBet.Equals(obj);

        public override string ToString() => _innerBet.ToString()!;
    }


    private const int ProgressionTypeShift = 0;
    private const ulong ProgressionTypeMask = 0b11 << ProgressionTypeShift;

    private const int BetTypeShift = 2;
    private const ulong BetTypeMask = 0b111 << BetTypeShift;

    private const ulong AvenueBetType = 0b000;
    private const ulong StreetBetType = 0b001;
    private const ulong BasketBetType = 0b010;
    private const ulong CornerBetType = 0b011;
    private const ulong SplitBetType = 0b100;
    private const ulong StraightUpBetType = 0b101;
    private const ulong HalfBetType = 0b110;
    private const ulong ThirdBetType = 0b111;

    private const int BetValueShift = 5;
    private const ulong BetValueMask = 0b111111 << BetValueShift;

    private ulong BetType => (Value & BetTypeMask) >> BetTypeShift;
    private ulong BetValue => (Value & BetValueMask) >> BetValueShift;

    public void Step(WagerDisposition wagerDisposition)
    {
        // TODO
    }

    public IBet ParseBet() => BetType switch
    {
        AvenueBetType => GetAvenueBet(),
        StreetBetType => GetStreetBet(),
        BasketBetType => GetBasketBet(),
        CornerBetType => GetCornerBet(),
        SplitBetType => GetSplitBet(),
        StraightUpBetType => GetStraightUpBet(),
        HalfBetType => GetHalfBet(),
        ThirdBetType => GetThirdBet(),
        _ => throw new NotImplementedException(),
    };

    private IBet GetAvenueBet()
    {
        const int avenueRowCount = 11;

        var avenue = (int)(BetValue % avenueRowCount * 3) + 1;
        return new Bet(BetFactory.Avenue(avenue), this);
    }

    private IBet GetStreetBet()
    {
        const int streetRowCount = 12;

        var street = (int)(BetValue % streetRowCount * 3) + 1;
        return new Bet(BetFactory.Street(street), this);
    }

    private IBet GetBasketBet() =>
        new Bet(BetFactory.Basket(), this);

    private IBet GetCornerBet()
    {
        const int cornerCount = 22;

        var finder = (int)(BetValue % cornerCount);
        var corner = (finder / 2 * 3) + (finder % 2) + 1;
        return new Bet(BetFactory.Corner(corner), this);
    }

    private IBet GetSplitBet()
    {
        const int rightCount = 23;
        const int downCount = 33;

        var finder = (int)(BetValue % (rightCount + downCount));

        IBet bet;
        if (finder == 0)
        {
            bet = BetFactory.SplitRight(finder);
        }
        else if (finder < rightCount)
        {
            finder--;
            var split = (finder / 2 * 3) + (finder % 2) + 1;
            bet = BetFactory.SplitRight(split);
        }
        else
        {
            finder -= rightCount;
            var split = finder + 1;
            bet = BetFactory.SplitDown(split);
        }

        return new Bet(bet, this);
    }

    private IBet GetStraightUpBet()
    {
        const int straightUpCount = 38;
        const int doubleZero = 100;

        var finder = (int)(BetValue % straightUpCount);
        var bet = BetFactory.StraightUp(finder == straightUpCount - 1 ? doubleZero : finder);
        return new Bet(bet, this);
    }

    private IBet GetHalfBet()
    {
        var finder = (int)(BetValue % 6);

        var bet = finder switch
        {
            0 => BetFactory.High(),
            1 => BetFactory.Low(),
            2 => BetFactory.Even(),
            3 => BetFactory.Odd(),
            4 => BetFactory.Red(),
            5 => BetFactory.Black(),
            _ => throw new NotImplementedException()
        };

        return new Bet(bet, this);
    }

    private IBet GetThirdBet()
    {
        var finder = (int)(BetValue % 6);
        var bet = finder switch
        {
            0 => BetFactory.Column(1),
            1 => BetFactory.Column(2),
            2 => BetFactory.Column(3),
            3 => BetFactory.Dozen(1),
            4 => BetFactory.Dozen(13),
            5 => BetFactory.Dozen(25),
            _ => throw new NotImplementedException()
        };

        return new Bet(bet, this);
    }
}
