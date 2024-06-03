using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = System.Random;

public static class Constants {
    public const int MaxPlayersAmount = 4;
    public const int MinPlayersAmount = 2;
    public const int MaxNameLength = 15;
    public const int MinNameLength = 1;
    
    public static readonly Random Rand = new();
    
    public const int Salary = 100;
    public const int StartCapital = 300;

    public static readonly Dictionary<string, string> flagNamesByCountryNames = new() {
        { "France", "Flag_of_France" },
        { "Germany", "Flag_of_Germany" },
        { "Japan", "Flag_of_Japan" },
        { "Ukraine", "Flag_of_Ukraine" },
    };
    
    public static readonly Dictionary<string, string> countryNamesByCountryNames = new() {
        { "France", "Франція" },
        { "Germany", "Німеччина" },
        { "Japan", "Японія" },
        { "Ukraine", "Україна" },
    };
    
    public static readonly string[] botsNames = {
        "Випадковий",
        "Жадібний",
        "Безвідмовний",
    };
    
    public static readonly AIBot[] botsClasses = {
        new ZhlobBot(),
        new AlwaysAgreeIfCanBot(),
    };

    public static AIBot GetPlayerBot(TMP_Dropdown dropdown) {
        if (dropdown == null) {
            return null;
        }
        if (dropdown.value == 0) {
            return botsClasses[Rand.Next(botsClasses.Length)];
        }
        return botsClasses[dropdown.value - 1];
    }

    public const string CurTurnBegin = "Зараз ходить ";
    public const string StartGameButtonText = "Починаймо грати!";
    public const string RollDiceButtonText = "Кинути кубик";
    public const string NextTurnButtonText = "Наступний хід";
    public const string BankruptButtonText = "Я банкрот";

    public static string GetCardNameByPos(Position position) {
        if (position == null) {
            return "Старт";
        }

        return position.cellIndex switch {
            2 => "Бонус",
            4 => "Зрада",
            6 => "Робота",
            9 => "Тюрма",
            13 => "Шанс виходу",
            17 => "Огляд",
            _ => "Підприємство"
        };
    }
    
    public static string EnterprisesWindowText(string purpose) {
        return purpose switch {
            "show" => "Підприємства, якими володіє гравець",
            "pawn" => "Натисніть на підприємства, які хочете закласти у банк",
            "unpawn" => "Натисніть на підприємства, які хочете викупити з банку",
            "build" => "Натисніть на підприємства, де хочете побудувати будинок",
            _ => "unknowm tag"
        };
    }
    public static string ButtonsEnterprisesWindow(string purpose) {
        return purpose switch {
            "pawn" => "Закласти підприємства",
            "unpawn" => "Викупити підприємства",
            "build" => "Побудувати будинки",
            _ => "unknowm tag"
        };
    }

    public static string TextMoneyEnterprisesWindow(string purpose) {
        return purpose switch {
            "pawn" => "Отримаєте:",
            "unpawn" or "build" => "Витратите:",
            _ => "unknowm tag"
        };
    }

    public static string FullSellNameByPos(Position position, Field field) {
        string cellName;
        string countryName;

        cellName = GetCardNameByPos(position);
        if (cellName == "Підприємство") {
            cellName = (field.TakeCardByPlayerPos(position) as Enterprise)?.title;
        }
        
        countryName = position?.arrayIndex is 0 or 1
            ? countryNamesByCountryNames[field.countriesArray[position.arrayIndex]] 
            : "невідомо";

        return cellName + " в країні " + countryName;
    }
    
    // Colors
    public const double PlayersColorsSimilarityDegree = 0.5;
    public static readonly Color PrimaryPriceColor = Gain256Color(220, 140, 80);
    public static readonly Color CurrentPriceColor = Gain256Color(230, 180, 100);
    public static readonly Color DefaultOutlineColor = Gain256Color(50, 10, 0);
    public static readonly Color SelectedOutlineColor = Gain256Color(255, 255, 255);

    private static Color Gain256Color(float red, float green, float blue, float alpha = 255) {
        return new Color(red / 255, green / 255, blue / 255, alpha / 255);
    }

    public static Color RandColor() {
        Color[] arr = {
            //Gain256Color(0, 0, 0), // Black
            Gain256Color(255, 255, 255), // White
            Gain256Color(255, 0, 0), // Red
            Gain256Color(0, 255, 0), // Lime
            //Gain256Color(0, 0, 255), // Blue
            Gain256Color(255, 255, 0), // Yellow
            Gain256Color(0, 255, 255), // Cyan / Aqua
            Gain256Color(255, 0, 255), // Magenta / Fuchsia
            Gain256Color(192, 192, 192), // Silver
            Gain256Color(128, 0, 0), // Maroon
            Gain256Color(128, 128, 0), // Olive
            Gain256Color(0, 128, 0), // Green
            Gain256Color(128, 0, 128), // Purple
            Gain256Color(0, 128, 128) // Teal
        };
        return arr[Rand.Next(arr.Length)];
    }

    public static int RollDice() {
        return Rand.Next(1, 7);
    }
    public static bool RollCoin(int trueProbability, int falseProbability) {
        return Rand.Next(0, trueProbability + falseProbability) - trueProbability < 0;
    }
    
}