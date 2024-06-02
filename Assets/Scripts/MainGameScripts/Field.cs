using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class Field
{
    public Card[][] fieldArrays;
    public Card startCell;
    public Industry[] industriesArray;
    public string[] countriesArray;

    // Struct of array #1: |ec01|ec02|bonus|ec02|zrada|ec02|money|ec03|ec03|prison|ec03|ec01|ec01|chance|ei1|ei1|ei2|review|ei2|ei1|ei3|
    // Indexes:            | 0  | 1  |  2  | 3  |  4  | 5  |  6  | 7  | 8  |  9   | 10 | 11 | 12 |  13  |14 |15 |16 |  17  |18 |19 |20 |
    // Country industries: | 00 | 01 |     | 01 |     | 01 |     | 02 | 02 |      | 02 | 00 | 00 |      |   |   |   |      |   |   |   |
    // Inter. industries:  |    |    |     |    |     |    |     |    |    |      |    |    |    |      | 1 | 1 | 2 |      | 2 | 1 | 3 |
    // Struct of array #2: |ec11|ec12|bonus|ec12|zrada|ec12|money|ec13|ec13|prison|ec13|ec11|ec11|chance|ei4|ei4|ei2|review|ei2|ei4|ei3|
    // Indexes:            | 0  | 1  |  2  | 3  |  4  | 5  |  6  | 7  | 8  |  9   | 10 | 11 | 12 |  13  |14 |15 |16 |  17  |18 |19 |20 |
    // Country industries: | 10 | 11 |     | 11 |     | 11 |     | 12 | 12 |      | 12 | 10 | 10 |      |   |   |   |      |   |   |   |
    // Inter. industries:  |    |    |     |    |     |    |     |    |    |      |    |    |    |      | 4 | 4 | 2 |      | 2 | 4 | 3 |
    // Start:              null
    private const int countriesAmount = 2;
    public const int arrayLength = 21;
    public const int enterOnArrayInAnother = 6;

    public static readonly Dictionary<int, string> specialCellNamesByIndexes = new() {
        { 2, "Bonus" },
        { 4, "Zrada" },
        { 6, "Work" },
        { 9, "Prison" },
        { 13, "ExitChance" },
        { 17, "Review" },
    };

    public static readonly Dictionary<string, int> specialIndexesByCellNames = new() {
        { "Start", -1},
        { "Bonus", 2 },
        { "Zrada", 4 },
        { "Work", 6 },
        { "Prison", 9 },
        { "ExitChance", 13 },
        { "Review", 17 },
    };

    private readonly int[][] countryIndustriesIndexes = {
        new[] { 11, 12, 0 },
        new[] { 1, 3, 5 },
        new[] { 7, 8, 10 }
    };

    private readonly int[][] commonIndustriesForEachFourEnters = {
        new[] { 16, 18 }
    };

    private readonly int[][] commonIndustriesForEachTwoEnters = {
        new[] { 20 }
    };

    private readonly int[][] privateIndustriesForEach = {
        new[] { 14, 15, 19 }
    };

    public Field() {
        fieldArrays = new Card[countriesAmount][];
        for (int i = 0; i < countriesAmount; i++) {
            fieldArrays[i] = new Card[arrayLength];
        }

        startCell = new StartCell();

        countriesArray = new string[countriesAmount];
        int industriesArrLength = (countryIndustriesIndexes.Length + privateIndustriesForEach.Length) * 2 +
                                  commonIndustriesForEachFourEnters.Length + commonIndustriesForEachTwoEnters.Length;
        industriesArray = new Industry[industriesArrLength];

        int curIndustryArrIndex = 0;
        string startOfTextFiles = "Assets/Resources/text_info";
        string nameOfCountryDir = startOfTextFiles + "/" + "enterprises_for_countries";
        string nameOfInterDir = startOfTextFiles + "/" + "enterprises_for_international";

        CountryIndustriesFill(nameOfCountryDir, ref curIndustryArrIndex);
        InternationalIndustriesFill(nameOfInterDir, ref curIndustryArrIndex);
        NonEnterprisesCardFill();

        GameShowManager.Instance.FieldToShow.SetStartValues(this);
    }

    public Card TakeCardByPlayerPos(Position position) {
        return position == null
            ? startCell
            : fieldArrays[position.arrayIndex][position.cellIndex];
    }

    private void NonEnterprisesCardFill() {
        foreach (var array in fieldArrays) {
            for (int i = 0; i < array.Length; i++) {
                if (specialCellNamesByIndexes.ContainsKey(i)) {
                    string cellType = specialCellNamesByIndexes[i];
                    array[i] = cellType switch {
                        "Bonus" => new Bonus(),
                        "Zrada" => new Zrada(),
                        "Work" => new Work(),
                        "Prison" => new Prison(),
                        "ExitChance" => new ExitChance(),
                        "Review" => new Review()
                    };
                }
            }
        }
    }

    private void CountryIndustriesFill(string countryDirName, ref int curIndustryArrIndex) {
        string[] countries = Directory.GetDirectories(countryDirName);
        List<int> countryIndexesInFile =
            ChooseNonRepeatableNums(0, countries.Length, fieldArrays.Length);

        for (int i = 0; i < countriesArray.Length; i++) {
            string countryFileName = countries[countryIndexesInFile[i]];
            countriesArray[i] = GetLastWordAfterSlashForDirectories(countryFileName);
            string[] industries = Directory.GetFiles(countryFileName, "*.txt");

            List<int> industriesIndexes =
                ChooseNonRepeatableNums(0, industries.Length, countryIndustriesIndexes.Length);
            List<int> arraysIndexesToFill = new() { i };

            FillIndustriesInField(industriesIndexes, countryIndustriesIndexes, arraysIndexesToFill,
                ref curIndustryArrIndex, industries);
        }
    }

    private void InternationalIndustriesFill(string internDirName, ref int curIndustryArrIndex) {
        string[] industriesDirs = Directory.GetDirectories(internDirName);
        int industriesNeeded;
        List<int> internationalIndustriesIndexes;
        List<int> industriesList = new List<int>();
        List<int> arraysIndexesToFill;

        string[] industries = Directory.GetFiles(industriesDirs[1], "*.txt");
        industriesNeeded = 2 * privateIndustriesForEach.Length;
        internationalIndustriesIndexes = ChooseNonRepeatableNums(0, industries.Length, industriesNeeded);
        for (int i = 0; i < fieldArrays.Length; i++) {
            for (int k = 0; k < privateIndustriesForEach.Length; k++) {
                industriesList.Add(internationalIndustriesIndexes[k + i * privateIndustriesForEach.Length]);
            }

            arraysIndexesToFill = new List<int>() { i };
            FillIndustriesInField(industriesList, privateIndustriesForEach, arraysIndexesToFill,
                ref curIndustryArrIndex, industries);
            industriesList.Clear();
        }

        industries = Directory.GetFiles(industriesDirs[0], "*.txt");
        industriesNeeded = commonIndustriesForEachTwoEnters.Length;
        internationalIndustriesIndexes = ChooseNonRepeatableNums(0, industries.Length, industriesNeeded);
        arraysIndexesToFill = new List<int>();

        for (int i = 0; i < commonIndustriesForEachTwoEnters.Length; i++) {
            industriesList.Add(internationalIndustriesIndexes[i]);
        }

        for (int i = 0; i < fieldArrays.Length; i++) {
            arraysIndexesToFill.Add(i);
        }

        FillIndustriesInField(industriesList, commonIndustriesForEachTwoEnters, arraysIndexesToFill,
            ref curIndustryArrIndex, industries);
        industriesList.Clear();


        industries = Directory.GetFiles(industriesDirs[2], "*.txt");
        industriesNeeded = commonIndustriesForEachFourEnters.Length;
        internationalIndustriesIndexes = ChooseNonRepeatableNums(0, industries.Length, industriesNeeded);
        arraysIndexesToFill = new List<int>();

        for (int i = 0; i < commonIndustriesForEachFourEnters.Length; i++) {
            industriesList.Add(internationalIndustriesIndexes[i]);
        }

        for (int i = 0; i < fieldArrays.Length; i++) {
            arraysIndexesToFill.Add(i);
        }

        FillIndustriesInField(industriesList, commonIndustriesForEachFourEnters, arraysIndexesToFill,
            ref curIndustryArrIndex, industries);
    }

    private void FillIndustriesInField(List<int> industriesIndexes, int[][] enterprisesIndexesInField,
        List<int> arraysIndexesToFill, ref int curIndustryIndexInGeneralArray, string[] industries) {
        for (int i = 0; i < industriesIndexes.Count; i++) {
            string currentIndustryDir = industries[industriesIndexes[i]];
            string currentIndustryName = GetLastWordAfterSlashForTxtFiles(currentIndustryDir);
            string[] curIndustryFile = File.ReadAllLines(currentIndustryDir);
            int arraysAmount = arraysIndexesToFill.Count;

            int enterprisesAmount = enterprisesIndexesInField[i].Length;
            List<Position> curIndustry = new List<Position>();
            industriesArray[curIndustryIndexInGeneralArray] = new Industry(curIndustry, currentIndustryName,
                UniqueColorForIndustry(curIndustryIndexInGeneralArray));

            int startPrice = Convert.ToInt32(curIndustryFile[0].Substring(0, curIndustryFile[0].IndexOf('-')));
            int endPrice = Convert.ToInt32(curIndustryFile[0].Substring(curIndustryFile[0].IndexOf('-') + 1));
            int stepPrice = (endPrice - startPrice) / enterprisesAmount / arraysAmount;

            List<int> enterpriseIndexesInFile =
                ChooseNonRepeatableNums(1, curIndustryFile.Length, enterprisesAmount * arraysAmount);

            for (int j = 0; j < arraysAmount; j++) {
                for (int k = 0; k < enterprisesAmount; k++) {
                    int curPrice = Constants.Rand.Next(startPrice, startPrice + stepPrice);
                    startPrice += stepPrice;
                    curPrice = curPrice / 10 * 10;

                    fieldArrays[arraysIndexesToFill[j]][enterprisesIndexesInField[i][k]] = new Enterprise(curPrice,
                        industriesArray[curIndustryIndexInGeneralArray], curIndustryFile[enterpriseIndexesInFile[j * enterprisesAmount + k]],
                        new Position(arraysIndexesToFill[j], enterprisesIndexesInField[i][k]));

                    curIndustry.Add(new Position(arraysIndexesToFill[j], enterprisesIndexesInField[i][k]));
                }
            }

            curIndustryIndexInGeneralArray++;
        }
    }

    private string GetLastWordAfterSlashForDirectories(string fileDirection) {
        return fileDirection.Substring(fileDirection.LastIndexOf("\\") + 1);
    }

    private string GetLastWordAfterSlashForTxtFiles(string fileDirection) {
        int lastIndexOfSlash = fileDirection.LastIndexOf("\\");
        return fileDirection.Substring(lastIndexOfSlash + 1, fileDirection.LastIndexOf(".") - (lastIndexOfSlash + 1));
    }

    private List<int> ChooseNonRepeatableNums(int begin, int end, int amount) {
        if (end - begin < amount) {
            return null;
        }

        List<int> ans = new List<int>();
        bool[] isUsed = new bool[end - begin];
        do {
            int curNum = Constants.Rand.Next(begin, end);
            if (isUsed[curNum - begin]) {
                continue;
            }

            ans.Add(curNum);
            isUsed[curNum - begin] = true;
        } while (ans.Count < amount);

        return ans;
    }

    private Color UniqueColorForIndustry(int indexCurIndustry) {
        Color color;
        bool isUnique;

        do {
            color = Constants.RandColor();
            isUnique = true;

            for (int i = 0; i < indexCurIndustry; i++) {
                if (color == industriesArray[i].color) {
                    isUnique = false;
                    break;
                }
            }
        } while (!isUnique);

        return color;
    }
}
