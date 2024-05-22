using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactive : MonoBehaviour
{
    private static readonly int minPlayersAmount = 2;
    private static readonly int maxPlayersAmount = 4;
    private static readonly int maxPlayerNameLength = 11;

    public static string GetPersonChoice(List<string> inputVariants) {
        string? strToReturn = null;

        do {
            if (strToReturn != null) {
                Console.WriteLine("Спробуйте ще раз ^_^\n");
            }

            Console.Write("Ваш вибір: ");
            strToReturn = Console.ReadLine();
        } while (!IsExistInList(strToReturn, inputVariants));

        return strToReturn;
    }

    public static void PressEnter() {
        Console.ReadLine();
    }

    public static string InputYourName() {
        string name;
        bool isCorrect;

        do {
            isCorrect = true;
            Console.Write("Введіть ваше ім'я: ");
            name = Console.ReadLine();

            if (name.Length > 15) {
                Console.WriteLine("Ім'я надто велике, спробуйте ще\n");
                isCorrect = false;
            }
            else if (name.Length == 0) {
                Console.WriteLine("У вас не може не бути імені\n");
                isCorrect = false;
            }
        } while (!isCorrect);

        return name;
    }

    public static string[] InputPlayersNamesToPlay() {
        bool isContinue = true;
        int playersAmount = 0;

        string[] previousRes = new string[maxPlayersAmount];

        Console.WriteLine("\nВи можете додати від 2 до 4 гравців.");
        while (isContinue) {
            Console.Write("\nВведіть 1, якщо хочете додати друга для гри або 0, якщо більше не хочете: ");

            switch (Console.ReadLine()) {
                case "1":
                    previousRes[playersAmount] = AddNamePlayer(previousRes, playersAmount);
                    playersAmount++;
                    if (playersAmount == maxPlayersAmount) {
                        isContinue = false;
                    }

                    break;
                case "0":
                    if (playersAmount < minPlayersAmount) {
                        Console.WriteLine("У грі мають брати участь не менше 2 людей, додайте гравців\n");
                    }
                    else {
                        isContinue = false;
                    }

                    break;
                default:
                    Console.WriteLine("Спробуйте ще раз ^_^\n");
                    break;
            }
        }

        string[] res = new string[playersAmount];
        Array.Copy(previousRes, 0, res, 0, playersAmount);
        return res;
    }

    private static string AddNamePlayer(string[] playerNames, int playersAmount) {
        string name;
        bool isCorrect;

        do {
            isCorrect = true;
            Console.Write($"Введіть ім'я гравця під номером {playersAmount + 1}: ");
            name = Console.ReadLine();

            if (name.Length > 15) {
                Console.WriteLine("Ім'я надто велике, спробуйте ще\n");
                isCorrect = false;
                continue;
            }

            if (name.Length == 0) {
                Console.WriteLine("У вас не може не бути імені\n");
                isCorrect = false;
                continue;
            }

            for (int i = 0; i < playersAmount; i++) {
                if (name == playerNames[i]) {
                    Console.WriteLine("Однакових імен у гравців бути не може, спробуйте ще\n");
                    isCorrect = false;
                }
            }
        } while (!isCorrect);

        return name;
    }

    private static bool IsExistInList(string strToCheck, List<string> inputVariants) {
        bool isCorrect = false;
        foreach (var str in inputVariants) {
            if (strToCheck == str) {
                return true;
            }
        }

        return false;
    }
}
