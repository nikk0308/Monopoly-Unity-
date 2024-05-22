using System;

public static class Constants {
    public const int MaxPlayersAmount = 6;
    public const int MinPlayersAmount = 2;
    public const int MaxNameLength = 15;
    public const int MinNameLength = 1;
    
    public static readonly Random Rand = new();



    public static int RollDice() {
        return Rand.Next(1, 7);
    }
    public static bool RollCoin(int trueProbability, int falseProbability) {
        return Rand.Next(0, trueProbability + falseProbability) - trueProbability < 0;
    }
}