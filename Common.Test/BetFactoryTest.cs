namespace Common.Test;

using RouletteStrategySimulator;

public class BetFactoryTest
{
    private static void AssertHits(IBet bet, params int[] hits)
    {
        foreach (var value in Enumerable.Range(0, 37).Concat(new[] { 100 }))
        {
            var number = RouletteBoard.Numbers[value];
            if (hits.Contains(value))
            {
                Assert.True(bet.Resolve(number));
            }
            else
            {
                Assert.False(bet.Resolve(number));
            }
        }
    }

    [Fact]
    public void BetFactoryCachesResults()
    {
        var a = BetFactory.Red();
        var b = BetFactory.Red();

        Assert.Same(a, b);
    }

    [Fact]
    public void CanGenerateAllBets()
    {
        var red = BetFactory.Red();
        AssertHits(red, 1, 3, 5, 7, 9, 12, 14, 16, 18, 19, 21, 23, 25, 27, 30, 32, 34, 36);

        var black = BetFactory.Black();
        AssertHits(black, 2, 4, 6, 8, 10, 11, 13, 15, 17, 20, 22, 24, 26, 28, 29, 31, 33, 35);

        var even = BetFactory.Even();
        AssertHits(even, 2, 4, 6, 8, 10, 12, 14, 16, 18, 20, 22, 24, 26, 28, 30, 32, 34, 36);

        var odd = BetFactory.Odd();
        AssertHits(odd, 1, 3, 5, 7, 9, 11, 13, 15, 17, 19, 21, 23, 25, 27, 29, 31, 33, 35);

        var high = BetFactory.High();
        AssertHits(high, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36);

        var low = BetFactory.Low();
        AssertHits(low, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18);

        var fiveNumber = BetFactory.Basket();
        AssertHits(fiveNumber, 0, 1, 2, 3, 100);

        var dozen1 = BetFactory.Dozen(1);
        AssertHits(dozen1, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12);

        var dozen2 = BetFactory.Dozen(13);
        AssertHits(dozen2, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24);

        var dozen3 = BetFactory.Dozen(25);
        AssertHits(dozen3, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36);

        var column1 = BetFactory.Column(1);
        AssertHits(column1, 1, 4, 7, 10, 13, 16, 19, 22, 25, 28, 31, 34);

        var column2 = BetFactory.Column(2);
        AssertHits(column2, 2, 5, 8, 11, 14, 17, 20, 23, 26, 29, 32, 35);

        var column3 = BetFactory.Column(3);
        AssertHits(column3, 3, 6, 9, 12, 15, 18, 21, 24, 27, 30, 33, 36);

        var sixLine1 = BetFactory.Avenue(1);
        AssertHits(sixLine1, 1, 2, 3, 4, 5, 6);

        var sixLine2 = BetFactory.Avenue(4);
        AssertHits(sixLine2, 4, 5, 6, 7, 8, 9);

        var sixLine3 = BetFactory.Avenue(7);
        AssertHits(sixLine3, 7, 8, 9, 10, 11, 12);

        var sixLine4 = BetFactory.Avenue(10);
        AssertHits(sixLine4, 10, 11, 12, 13, 14, 15);

        var sixLine5 = BetFactory.Avenue(13);
        AssertHits(sixLine5, 13, 14, 15, 16, 17, 18);

        var sixLine6 = BetFactory.Avenue(16);
        AssertHits(sixLine6, 16, 17, 18, 19, 20, 21);

        var sixLine7 = BetFactory.Avenue(19);
        AssertHits(sixLine7, 19, 20, 21, 22, 23, 24);

        var sixLine8 = BetFactory.Avenue(22);
        AssertHits(sixLine8, 22, 23, 24, 25, 26, 27);

        var sixLine9 = BetFactory.Avenue(25);
        AssertHits(sixLine9, 25, 26, 27, 28, 29, 30);

        var sixLine10 = BetFactory.Avenue(28);
        AssertHits(sixLine10, 28, 29, 30, 31, 32, 33);

        var sixLine11 = BetFactory.Avenue(31);
        AssertHits(sixLine11, 31, 32, 33, 34, 35, 36);

        var corner1 = BetFactory.Corner(1);
        AssertHits(corner1, 1, 2, 4, 5);

        var corner2 = BetFactory.Corner(2);
        AssertHits(corner2, 2, 3, 5, 6);

        var corner3 = BetFactory.Corner(4);
        AssertHits(corner3, 4, 5, 7, 8);

        var corner4 = BetFactory.Corner(5);
        AssertHits(corner4, 5, 6, 8, 9);

        var corner5 = BetFactory.Corner(7);
        AssertHits(corner5, 7, 8, 10, 11);

        var corner6 = BetFactory.Corner(8);
        AssertHits(corner6, 8, 9, 11, 12);

        var corner7 = BetFactory.Corner(10);
        AssertHits(corner7, 10, 11, 13, 14);

        var corner8 = BetFactory.Corner(11);
        AssertHits(corner8, 11, 12, 14, 15);

        var corner9 = BetFactory.Corner(13);
        AssertHits(corner9, 13, 14, 16, 17);

        var corner10 = BetFactory.Corner(14);
        AssertHits(corner10, 14, 15, 17, 18);

        var corner11 = BetFactory.Corner(16);
        AssertHits(corner11, 16, 17, 19, 20);

        var corner12 = BetFactory.Corner(17);
        AssertHits(corner12, 17, 18, 20, 21);

        var corner13 = BetFactory.Corner(19);
        AssertHits(corner13, 19, 20, 22, 23);

        var corner14 = BetFactory.Corner(20);
        AssertHits(corner14, 20, 21, 23, 24);

        var corner15 = BetFactory.Corner(22);
        AssertHits(corner15, 22, 23, 25, 26);

        var corner16 = BetFactory.Corner(23);
        AssertHits(corner16, 23, 24, 26, 27);

        var corner17 = BetFactory.Corner(25);
        AssertHits(corner17, 25, 26, 28, 29);

        var corner18 = BetFactory.Corner(26);
        AssertHits(corner18, 26, 27, 29, 30);

        var corner19 = BetFactory.Corner(28);
        AssertHits(corner19, 28, 29, 31, 32);

        var corner20 = BetFactory.Corner(29);
        AssertHits(corner20, 29, 30, 32, 33);

        var corner21 = BetFactory.Corner(31);
        AssertHits(corner21, 31, 32, 34, 35);

        var corner22 = BetFactory.Corner(32);
        AssertHits(corner22, 32, 33, 35, 36);

        var splitRight0 = BetFactory.SplitRight(0);
        AssertHits(splitRight0, 0, 100);

        var splitRight1 = BetFactory.SplitRight(1);
        AssertHits(splitRight1, 1, 2);

        var splitRight2 = BetFactory.SplitRight(2);
        AssertHits(splitRight2, 2, 3);

        var splitRight3 = BetFactory.SplitRight(4);
        AssertHits(splitRight3, 4, 5);

        var splitRight4 = BetFactory.SplitRight(5);
        AssertHits(splitRight4, 5, 6);

        var splitRight5 = BetFactory.SplitRight(7);
        AssertHits(splitRight5, 7, 8);

        var splitRight6 = BetFactory.SplitRight(8);
        AssertHits(splitRight6, 8, 9);

        var splitRight7 = BetFactory.SplitRight(10);
        AssertHits(splitRight7, 10, 11);

        var splitRight8 = BetFactory.SplitRight(11);
        AssertHits(splitRight8, 11, 12);

        var splitRight9 = BetFactory.SplitRight(13);
        AssertHits(splitRight9, 13, 14);

        var splitRight10 = BetFactory.SplitRight(14);
        AssertHits(splitRight10, 14, 15);

        var splitRight11 = BetFactory.SplitRight(16);
        AssertHits(splitRight11, 16, 17);

        var splitRight12 = BetFactory.SplitRight(17);
        AssertHits(splitRight12, 17, 18);

        var splitRight13 = BetFactory.SplitRight(19);
        AssertHits(splitRight13, 19, 20);

        var splitRight14 = BetFactory.SplitRight(20);
        AssertHits(splitRight14, 20, 21);

        var splitRight15 = BetFactory.SplitRight(22);
        AssertHits(splitRight15, 22, 23);

        var splitRight16 = BetFactory.SplitRight(23);
        AssertHits(splitRight16, 23, 24);

        var splitRight17 = BetFactory.SplitRight(25);
        AssertHits(splitRight17, 25, 26);

        var splitRight18 = BetFactory.SplitRight(26);
        AssertHits(splitRight18, 26, 27);

        var splitRight19 = BetFactory.SplitRight(28);
        AssertHits(splitRight19, 28, 29);

        var splitRight20 = BetFactory.SplitRight(29);
        AssertHits(splitRight20, 29, 30);

        var splitRight21 = BetFactory.SplitRight(31);
        AssertHits(splitRight21, 31, 32);

        var splitRight22 = BetFactory.SplitRight(32);
        AssertHits(splitRight22, 32, 33);

        var splitRight23 = BetFactory.SplitRight(34);
        AssertHits(splitRight23, 34, 35);

        var splitRight24 = BetFactory.SplitRight(35);
        AssertHits(splitRight24, 35, 36);

        var splitDown1 = BetFactory.SplitDown(1);
        var splitDown2 = BetFactory.SplitDown(2);
        var splitDown3 = BetFactory.SplitDown(3);
        var splitDown4 = BetFactory.SplitDown(4);
        var splitDown5 = BetFactory.SplitDown(5);
        var splitDown6 = BetFactory.SplitDown(6);
        var splitDown7 = BetFactory.SplitDown(7);
        var splitDown8 = BetFactory.SplitDown(8);
        var splitDown9 = BetFactory.SplitDown(9);
        var splitDown10 = BetFactory.SplitDown(10);
        var splitDown11 = BetFactory.SplitDown(11);
        var splitDown12 = BetFactory.SplitDown(12);
        var splitDown13 = BetFactory.SplitDown(13);
        var splitDown14 = BetFactory.SplitDown(14);
        var splitDown15 = BetFactory.SplitDown(15);
        var splitDown16 = BetFactory.SplitDown(16);
        var splitDown17 = BetFactory.SplitDown(17);
        var splitDown18 = BetFactory.SplitDown(18);
        var splitDown19 = BetFactory.SplitDown(19);
        var splitDown20 = BetFactory.SplitDown(20);
        var splitDown21 = BetFactory.SplitDown(21);
        var splitDown22 = BetFactory.SplitDown(22);
        var splitDown23 = BetFactory.SplitDown(23);
        var splitDown24 = BetFactory.SplitDown(24);
        var splitDown25 = BetFactory.SplitDown(25);
        var splitDown26 = BetFactory.SplitDown(26);
        var splitDown27 = BetFactory.SplitDown(27);
        var splitDown28 = BetFactory.SplitDown(28);
        var splitDown29 = BetFactory.SplitDown(29);
        var splitDown30 = BetFactory.SplitDown(30);
        var splitDown31 = BetFactory.SplitDown(31);
        var splitDown32 = BetFactory.SplitDown(32);
        var splitDown33 = BetFactory.SplitDown(33);

        var straightUp0 = BetFactory.StraightUp(0);
        var straightUp00 = BetFactory.StraightUp(100);
        var straightUp1 = BetFactory.StraightUp(1);
        var straightUp2 = BetFactory.StraightUp(2);
        var straightUp3 = BetFactory.StraightUp(3);
        var straightUp4 = BetFactory.StraightUp(4);
        var straightUp5 = BetFactory.StraightUp(5);
        var straightUp6 = BetFactory.StraightUp(6);
        var straightUp7 = BetFactory.StraightUp(7);
        var straightUp8 = BetFactory.StraightUp(8);
        var straightUp9 = BetFactory.StraightUp(9);
        var straightUp10 = BetFactory.StraightUp(10);
        var straightUp11 = BetFactory.StraightUp(11);
        var straightUp12 = BetFactory.StraightUp(12);
        var straightUp13 = BetFactory.StraightUp(13);
        var straightUp14 = BetFactory.StraightUp(14);
        var straightUp15 = BetFactory.StraightUp(15);
        var straightUp16 = BetFactory.StraightUp(16);
        var straightUp17 = BetFactory.StraightUp(17);
        var straightUp18 = BetFactory.StraightUp(18);
        var straightUp19 = BetFactory.StraightUp(19);
        var straightUp20 = BetFactory.StraightUp(20);
        var straightUp21 = BetFactory.StraightUp(21);
        var straightUp22 = BetFactory.StraightUp(22);
        var straightUp23 = BetFactory.StraightUp(23);
        var straightUp24 = BetFactory.StraightUp(24);
        var straightUp25 = BetFactory.StraightUp(25);
        var straightUp26 = BetFactory.StraightUp(26);
        var straightUp27 = BetFactory.StraightUp(27);
        var straightUp28 = BetFactory.StraightUp(28);
        var straightUp29 = BetFactory.StraightUp(29);
        var straightUp30 = BetFactory.StraightUp(30);
        var straightUp31 = BetFactory.StraightUp(31);
        var straightUp32 = BetFactory.StraightUp(32);
        var straightUp33 = BetFactory.StraightUp(33);
        var straightUp34 = BetFactory.StraightUp(34);
        var straightUp35 = BetFactory.StraightUp(35);
        var straightUp36 = BetFactory.StraightUp(36);
    }
}
