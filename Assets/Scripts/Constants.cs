using System;

public static class Constants {
    public const int MaxPlayersAmount = 6;
    public const int MinPlayersAmount = 2;
    public const int MaxNameLength = 15;
    public const int MinNameLength = 1;

    public const int MillisecondsToShowError = 5000;
    
    public static Random Rand = new();
}