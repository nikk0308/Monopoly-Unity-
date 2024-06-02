using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class JustOutput : MonoBehaviour
{
    // Максимум в ширину — 186 символів
    public static readonly int maxCellsInOneLine = 8;
    public static readonly int maxSymbolsInOneCell = 15;
    public static readonly int screenWidth = 186;

    public static void PrintAListOfEnterprisesInOneLine(List<Enterprise> enterprises) {
        string[][] enterprisesInLines = new string[enterprises.Count][];
        int curBoard;

        for (int i = 0; i < enterprisesInLines.Length; i++) {
            //enterprisesInLines[i] = enterprises[i].TextToPrintInAField;
        }

        for (int i = 0; i < enterprisesInLines.Length; i += maxCellsInOneLine) {
            curBoard = Math.Min(enterprisesInLines.Length - i, maxCellsInOneLine);
            Console.WriteLine(OutputPhrases.CurWideLine(Math.Min(curBoard, maxCellsInOneLine), false));
            for (int h = 0; h < enterprisesInLines[0].Length; h++) {
                for (int k = 0; k < curBoard; k++) {
                    int freeSpace = maxSymbolsInOneCell - enterprisesInLines[k + i][h].Length;
                    Console.Write("|");
                    Console.Write(new string(' ', freeSpace / 2));
                    Console.Write(enterprisesInLines[k + i][h]);
                    Console.Write(new string(' ', freeSpace - freeSpace / 2));
                    Console.Write("| ");
                }

                Console.Write("\n");
            }

            Console.WriteLine(OutputPhrases.CurWideLine(Math.Min(curBoard, maxCellsInOneLine), true));
            Console.WriteLine();
        }
    }

    internal static void PrintText(string textToPrint) {
        Thread.Sleep(200);
        Console.WriteLine(textToPrint);
    }

    internal static void Congratulations(Player player, Field field) {
        Console.WriteLine("\n" + new string('-', screenWidth));
        Console.WriteLine(OutputPhrases.TextPlayerGreetings(player));
        PrintAListOfEnterprisesInOneLine(player.GetAllPlayerEnterprises(field));
        Console.WriteLine(new string('-', screenWidth) + "\n");
    }

    internal static void PrintPlayersInfo(List<Player> playersInGame, Field field) {
        Console.WriteLine();
        foreach (var player in playersInGame) {
            ConsoleColor fore = Console.ForegroundColor;
            //Console.ForegroundColor = player.chipColor;
            Console.Write(player.nameInGame);
            Console.ForegroundColor = fore;
            Console.WriteLine(OutputPhrases.TextPlayerInfo(player, field));
            PrintAListOfEnterprisesInOneLine(player.GetAllPlayerEnterprises(field));
        }

        Console.WriteLine();
    }

    internal static void PrintDebtAndUnPawnEnterprises(Player player, List<Enterprise> enterprises) {
        Console.WriteLine(OutputPhrases.TextDebtAndEnter(player));
        PrintAListOfEnterprisesInOneLine(enterprises);
    }

    internal static List<string> MakeAListFromDiapasone(int start, int end) {
        List<string> list = new List<string>();
        for (int i = start; i < end + 1; i++) {
            list.Add(Convert.ToString(i));
        }

        return list;
    }

    internal static void PrintEnterprises(List<Enterprise> enterprises, string tag) {
        Console.WriteLine(tag switch {
            "notPawned" => OutputPhrases.TextUnPawnedOrPawnedEnterprises(false),
            "pawned" => OutputPhrases.TextUnPawnedOrPawnedEnterprises(true),
            "hotel" => OutputPhrases.TextEnterprisesToBuildHotel(),
            _ => "unknown tag"
        });
        PrintAListOfEnterprisesInOneLine(enterprises);
    }

    internal static void PrintMyChoice() {
        Console.Write("Мій вибір: ");
    }

    public static void PrintAllField(Field field, List<Player> players) {
        List<int>[][] fieldIndexes = OutputPhrases.fieldIndexes;
        int cellWidth = OutputPhrases.maxCellWidth;
        int cellHeight = OutputPhrases.cellHeight;
        int sleepTime = 15;
        int chipWidth = 2;
        List<List<int>> positions = new List<List<int>>();
        List<List<Player>> playersOnPositions = new List<List<Player>>();
        PlayersPlacesInField(ref positions, ref playersOnPositions, players);

        Console.Write(" ");
        foreach (var list in fieldIndexes[0]) {
            if (OutputPhrases.IsNotBoard(list)) {
                Console.Write(new string(' ', cellWidth));
            }
            else {
                Console.Write(new string('_', cellWidth));
            }

            Console.Write(" ");
        }

        Thread.Sleep(sleepTime);
        Console.WriteLine();
        for (int l = 0; l < fieldIndexes.Length; l++) {
            for (int i = 0; i < cellHeight; i++) {
                if (OutputPhrases.IsNotBoard(fieldIndexes[l][0])) {
                    Console.Write(" ");
                }
                else {
                    Console.Write("|");
                }

                for (int k = 0; k < fieldIndexes[l].Length; k++) {
                    PrintOneStringInCell(field, fieldIndexes[l][k], i);
                    if (!OutputPhrases.IsNotBoard(fieldIndexes[l][k])) {
                        Console.Write("|");
                    }
                    else if (k < fieldIndexes[l].Length - 1 && !OutputPhrases.IsNotBoard(fieldIndexes[l][k + 1])) {
                        Console.Write("|");
                    }
                    else {
                        Console.Write(" ");
                    }
                }

                Thread.Sleep(sleepTime);
                Console.WriteLine();
            }

            if (OutputPhrases.IsNotBoard(fieldIndexes[l][0])) {
                Console.Write(" ");
            }
            else {
                Console.Write("|");
            }

            for (int i = 0; i < fieldIndexes[l].Length; i++) {
                if (OutputPhrases.IsNotBoard(fieldIndexes[l][i]) &&
                    (l == fieldIndexes.Length - 1 || OutputPhrases.IsNotBoard(fieldIndexes[l + 1][i]))) {
                    Console.Write(new string(' ', cellWidth));
                }
                else {
                    int index = GetPlayersByPos(fieldIndexes[l][i], positions, playersOnPositions);
                    if (index != -1) {
                        WritePlayersOnWideUnderline(playersOnPositions[index], cellWidth, chipWidth);
                    }
                    else {
                        Console.Write(new string('_', cellWidth));
                    }
                }

                if (!OutputPhrases.IsNotBoard(fieldIndexes[l][i])) {
                    Console.Write("|");
                }
                else if (i < fieldIndexes[l].Length - 1 && !OutputPhrases.IsNotBoard(fieldIndexes[l][i + 1])) {
                    Console.Write("|");
                }
                else {
                    Console.Write(" ");
                }
            }

            Thread.Sleep(sleepTime);
            Console.WriteLine();
        }
    }

    public static void PrintOneStringInCell(Field field, List<int> cellIndexes, int stringIndex) {
        string[] stringText = OutputPhrases.GetCellText(field, cellIndexes);
        if (cellIndexes[0] == 1 && cellIndexes[1] != -1 &&
            field.fieldArrays[cellIndexes[1]][cellIndexes[2]] is Enterprise enterprise) {
            if (stringIndex == 0) {
                ConsoleColor backColor = Console.BackgroundColor;
                ConsoleColor foreColor = Console.ForegroundColor;
                //Console.BackgroundColor = enterprise.industry.color;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write(stringText[stringIndex]);
                Console.BackgroundColor = backColor;
                Console.ForegroundColor = foreColor;
                return;
            }

            if (stringIndex == 3 && enterprise.owner != null) {
                ConsoleColor foreColor = Console.ForegroundColor;
                //Console.ForegroundColor = enterprise.owner.chipColor;
                Console.Write(stringText[stringIndex]);
                Console.ForegroundColor = foreColor;
                return;
            }
        }

        Console.Write(stringText[stringIndex]);
    }

    private static void PlayersPlacesInField(ref List<List<int>> positions, ref List<List<Player>> playersOnPositions,
        List<Player> players) {
        foreach (var player in players) {
            List<int> list = new List<int> {
                1,
                player.positionInField == null ? -1 : player.positionInField.arrayIndex,
                player.positionInField == null ? 0 : player.positionInField.cellIndex
            };

            int index = GetPlayersByPos(list, positions, playersOnPositions);
            if (index != -1) {
                playersOnPositions[index].Add(player);
            }
            else {
                positions.Add(list);
                playersOnPositions.Add(new List<Player> { player });
            }
        }
    }

    private static int GetPlayersByPos(List<int> pos, List<List<int>> positions,
        List<List<Player>> playersOnPositions) {
        for (int i = 0; i < positions.Count; i++) {
            if (AreListsEqual(positions[i], pos)) {
                return i;
            }
        }

        return -1;
    }

    private static bool AreListsEqual(List<int> fList, List<int> sList) {
        if (fList.Count != sList.Count) {
            return false;
        }

        for (int i = 0; i < fList.Count; i++) {
            if (fList[i] != sList[i]) {
                return false;
            }
        }

        return true;
    }

    private static void WritePlayersOnWideUnderline(List<Player> playersOnCell, int cellWidth, int chipWidth) {
        int playersAmount = playersOnCell.Count;
        int freeSpace = cellWidth - playersAmount * chipWidth;
        int betweenSpace = 0;
        if (playersAmount > 1) {
            betweenSpace = freeSpace / (playersAmount + 1);
        }

        int leftSpace = (cellWidth - (betweenSpace * (playersAmount - 1) + chipWidth * playersAmount)) / 2;
        int rightSpace = cellWidth - (leftSpace + betweenSpace * (playersAmount - 1) + chipWidth * playersAmount);

        ConsoleColor back = Console.BackgroundColor;

        Console.Write(new string('_', leftSpace));

        bool isFirst = true;
        foreach (var player in playersOnCell) {
            if (!isFirst) {
                Console.Write(new string('_', betweenSpace));
            }

            //Console.BackgroundColor = player.chipColor;
            Console.Write(new string('_', chipWidth));
            Console.BackgroundColor = back;
            isFirst = false;
        }

        Console.Write(new string('_', rightSpace));
    }
}
