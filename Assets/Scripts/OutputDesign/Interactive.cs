using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactive
{

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
