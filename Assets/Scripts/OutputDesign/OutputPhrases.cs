using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutputPhrases : MonoBehaviour
{
    
    //__________________________________________________________________________________________________________________________________________
    // CARDS

    public static string[] TextToShowEnterprise(Enterprise enterprise) {
        return new[] {
            enterprise.title,
            enterprise.priceOthersPayLevel1 + "|" + enterprise.priceOthersPayLevel2 + "|" + enterprise.priceOthersPayLevel3,
            "До сплати: " + enterprise.currentPriceOthersPay,
            (enterprise.owner == null) ? "-" : 
                (enterprise.owner.nameInGame + (enterprise.IsPawned() ? " (" + enterprise.turnsToDisappearIfPawned + ")" : "")),
        };
    }

    public static string TextBonusOrZradaMoneyAmount(Player player, int money, bool isBonus) {
        return player.nameInGame + " " + (isBonus ? "отримує" : "втрачає") + " " + money + " " + TextHryvnaEnding(money) + "!";
    }

    public static string TextNoGainOrTake(Player player) {
        return player.nameInGame + " нічого не отримує і не втрачає";
    }

    public static string ZradaOrBonusMovedTo(Player player, bool isBonus) {
        return "Внаслідок " + (isBonus ? "бонуса" : "зради") + " гравець " + player.nameInGame +
               " переміщується на клітинку " + Constants.GetCardNameByPos(player.positionInField);
    }
    
    public static string TextBuyEnterpriseOrNot(Player player, Enterprise enterprise) {
        return player.nameInGame + " зараз може купити " + enterprise.title + " за " + enterprise.priceToBuy + " гривень\n" +
                          "Придбаєте підприємство чи залишите його на покупку іншим гравцям?";
    }
    
    public static string TextPawnInBank(Enterprise enterprise) {
        return enterprise.title + " закладено у банк. " + enterprise.owner.nameInGame + " отримує " +
            enterprise.currentPriceOthersPay + " на свій рахунок\n";
    }
    public static string TextUnPawnFromBank(Enterprise enterprise) {
        return enterprise.title + " викуплено з банку гравцем " + enterprise.owner.nameInGame + "!\n";
    }
    public static string TextBuildHome(Enterprise enterprise) {
        return "Тепер " + enterprise.owner.nameInGame + " може відпочивати у своєму будинку біля " +
            enterprise.title + ", якщо буде тут проїздом\n";
    }

    public static string TextGainOrLostLocalMonopoly(Player player, Enterprise enterprise, bool isGain) {
        return player.nameInGame + " " +
            (isGain ? "стає локальним монополістом" : "втрачає локальну монополію") +
            " в індустрії " + enterprise.industry.industryName + "!\n";
    }

    public static string TextPayBuyOrStay(Player player, Enterprise enterprise, string tag) {
        return tag switch {
            "inHome" => "Яке щастя! У себе вдома " + player.nameInGame + " не зобов'язується платити комусь!",
            "inBank" => player.nameInGame + " щастить, так як картка на даний момент закладена у банк",
            "inPrison" => player.nameInGame + " щастить, власник картки зараз у тюрмі",
            "payAnotherPerson" => player.nameInGame + " заходить не на свою територію, тому передає " + enterprise.currentPriceOthersPay +
                       " гривень гравцю " + enterprise.owner.nameInGame,
            "noMoneyToBuy" => "Грошей на придбання " + enterprise.title + " (" + enterprise.priceToBuy + " гривень) у " + 
                        player.nameInGame + " немає, тому йдемо далі",
            "bought" => enterprise.title + " придбано гравцем " + player.nameInGame + "! Вітаємо!",
            "discard" => player.nameInGame + " стримує емоції та зберігає гроші на майбутні інвестиції",
            _ => "unknowm tag"
        };
    }

    public static string TextGoOutOrNot(Player player, bool isGoOut) {
        return player.nameInGame + " " + (isGoOut
            ? "нарешті виходить з країни та зараз ходить (^_^)"
            : "не виходить з країни і йде далі по колу (>_<)");
    }

    public static string TextGuessIsGoOut(Player player) {
        return "Вийде " + player.nameInGame + " з країни чи ні — пока що загадка.";
    }

    public static string TextGoToPrisonOrNot(Player player, bool isGoToPrison) {
        return player.nameInGame + " " + (isGoToPrison ?
             "не встигає сховати контрабанду та відправляється до тюрми :(" : "відкупляється від перевіряючих");
    }

    public static string TextWorkChoice(Player player) {
        return "До завершення роботи " + player.nameInGame + " має відробити ще " +
                          player.turnsCanContinueWork + " " + TextDayEnding(player.turnsCanContinueWork) +
                          ". \nПотрібно вирішити: відробити ще день на роботі або піти з неї";
    }

    public static string TextStartWork(Player player, bool canWork) {
        return canWork ?
            player.nameInGame + " отримує роботу на " + player.turnsCanContinueWork + " " +
                   TextDayEnding(player.turnsCanContinueWork) + ". Початок — завтра." :
            player.nameInGame + " зробив уже всі завдання на роботі. Роботодавець не може дати роботу гравцю";

    }

    public static string TextFinishWorking(Player player, bool isFinish) {
        return isFinish ?
            player.nameInGame + " відробляє повний термін на даний момент. Гравець може зробити хід далі" :
            player.nameInGame + " покидає роботу та може зробити хід далі, назустріч мрії";
    }

    public static string TextBossPayed(Player player, int salary) {
        return "Гарна робота, " + player.nameInGame + "! Роботодавець заплатив " + salary + " гривень за день роботи";
    }

    public static string TextIsPayedForFreedom(int priceToPay) {
        return "До вас підійшов охоронець та запропонував витягнути з тюрми за " + priceToPay + " гривень. Чи хочете ви " +
               "вийти з тюрми раніше відведеного вам строку?";
    }

    public static string TextSendPlayerToPrison(Player player, int turnsToGoOut) {
        return player.nameInGame + " потрапляє до в'язниці на " + turnsToGoOut + " " + TurnEnding(turnsToGoOut);
    }

    public static string TextTurnsRemaining(Player player, int turnsToGoOut) {
        return player.nameInGame + " залишилося відсидіти ще " + turnsToGoOut + " " + TurnEnding(turnsToGoOut);
    }

    public static string TextBuyFreedomOrNot(Player player, bool isBought) {
        return isBought ?
            player.nameInGame + " викупляє свободу!" :
            "На жаль, грошей для викупу виявилося недостатньо";
    }

    public static string TextGoOutOfPrisonOrNot(Player player, bool isGoOut) {
        return isGoOut ?
            player.nameInGame + " нарешті виходить із тюрми!" :
            player.nameInGame + " вирішив продовжувати відбувати покарання";
    }

    public static string MovedToStart(Player player) {
        return player.nameInGame + " переміщується на поле " + Constants.GetCardNameByPos(null);
    }
    //__________________________________________________________________________________________________________________________________________
    // GamePlay

    public static string PlayerMovedTo(Player player, Field field) {
        return player.nameInGame + " переміщується на " + PrintCellTitleInAText(field.TakeCardByPlayerPos(player.positionInField)) + "\n";
    }

    public static string TextStartTurn(Player player, Field field, int startMoney) {
        return player.nameInGame + " випадає доля йти в країну " +
               Constants.countryNamesByCountryNames[field.countriesArray[player.positionInField.arrayIndex]] +
               ". Видано стартові " + startMoney + " гривень на підняття економіки країни";
    }
    
    public static string TextPressEnterToGoNextPlayer() {
        return "Натисніть Enter щоб перейти до наступного гравця";
    }

    public static string TextGainSalary(Player player, int salaryAmount) {
        return player.nameInGame + " забігає швиденько на роботу, отримує премію в розмірі " + salaryAmount + " гривень";
    }

    public static string TextPlayerGreetings(Player player) {
        return "Вітаємо гравця " + player.nameInGame + " з абсолютною монополією та банком у розмірі " +
                          player.moneyAmount + " гривень!\n" +
                          "А також, наступними підприємствами:";
    }

    public static string TextPlayerInfo(Player player, Field field) {
        return " зараз має " + player.moneyAmount + " гривень. " +
                              "Знаходиться у " + GetCountryNameByPlayer(field, player) +
                              " на клітинці " + PrintCellTitleInAText(field.TakeCardByPlayerPos(player.positionInField)) +
                              ". Його підприємства: ";
    }

    public static string TextYouMustPawnEnterprises() {
        return "У вас не вистачає грошей для продовження гри. Ви маєте закласти у банк або продати якісь зі своїх" +
                          " підприємств або програєте";
    }

    public static string TextDebtAndEnter(Player player) {
        return "Борг банку складає " + player.moneyAmount * (-1) + 
            "\nВаші не закладені підприємства на даний момент:";
    }

    public static string TextPawnToNotLost(Player player, string tag) {
        return tag switch {
            "noEnough" => "Грошей для погашення боргу не вистачило\n",
            "backInGame" => "Борг вдалося погасити! Ви ще у грі!",
            "lost" => "На жаль, підприємства скінчилися, а борг не погашено. " + player.nameInGame + " вибуває з гри",
            _ => "unknowm tag"
        };
    }

    public static string TextInputEnterpriseNum(int enterprisesAmount) {
        return "Введіть номер підприємства (від 1 до " + enterprisesAmount + "): ";
    }

    public static string TextGetNumOfPreTurnAction() {
        return "Введіть\n" +
            "  1, якщо хочете закласти підприємство у банк\n" +
            "  2, якщо хочете викупити підприємство з банку\n" +
            "  3, якщо хочете побудувати готель на підприємстві\n" +
            "  0, якщо поки що нічого не хочете\n";
    }

    public static string TextPreTurnMainOutput(Player player, int notPawnedEnterprises, int pawnedEnterprises, int enterprisesToBuildHotel) {
        return player.nameInGame + " може перед ходом \n" +
                          "закласти підприємство (" + notPawnedEnterprises + " шт.) у банк, \n" +
                          "викупити підприємство (" + pawnedEnterprises + " шт.) з банку, \n" +
                          "побудувати готель     (" + enterprisesToBuildHotel + " шт.) на підприємстві\n";
    }

    public static string TextUnPawnedOrPawnedEnterprises(bool isPawned) {
        return "Ваші " + (isPawned ? "" : "не") + " закладені у банк підприємства:";
    }

    public static string TextEnterprisesToBuildHotel() {
        return "Ваші підприємства, на яких можна збудувати готель \n" + "" +
            "(для побудови необхідно в 3 рази більше грошей від покупки підприємства):";
    }

    public static string TextNoEnterprisesFor(string tag) {
        return "На даний момент немає підприємств для " +
            tag switch {
                "pawn" => "закладання у банк",
                "unPawn" => "викупу з банку",
                "hotel" => "будування готелю там",
                _ => "unknown tag",
            };
    }

    public static string TextNoMoneyForUnpawnOrBuild(bool isUnpawn) {
        return "На жаль, грошей для " + (isUnpawn ? "викупу" : "побудови готелю тут") + " не вистачає";
    }

    public static string PlayerTurns(Player player) {
        return player.nameInGame + " ходить.";
    }

    //__________________________________________________________________________________________________________________________________________
    // Field

    public static readonly List<int>[][] fieldIndexes = { // Struct: (is in field, which array in field, which cell in field)
        new [] { nL(0, 0, 0), nL(1, 0, 16), nL(1, 0, 17), nL(1, 0, 18), nL(1, 0, 19), nL(1, 0, 20), nL(1, 1, 6), nL(1, 1, 7), nL(1, 1, 8), nL(1, 1, 9), nL(1, 1, 10) },
        new [] { nL(1, 0, 15), nL(0, 0, 0), nL(0, 0, 0), nL(0, 0, 0), nL(0, 0, 0), nL(0, 0, 0), nL(1, 1, 5), nL(0, 0, 0), nL(0, 1, 1), nL(0, 0, 0), nL(1, 1, 11) },
        new [] { nL(1, 0, 14), nL(0, 0, 0), nL(0, 0, 0), nL(0, 0, 0), nL(0, 0, 0), nL(0, 0, 0), nL(1, 1, 4), nL(0, 0, 0), nL(0, 0, 0), nL(0, 0, 0), nL(1, 1, 12) },
        new [] { nL(1, 0, 13), nL(1, 0, 0), nL(1, 0, 1), nL(1, 0, 2), nL(1, 0, 3), nL(1, -1, 0), nL(1, 1, 3), nL(1, 1, 2), nL(1, 1, 1), nL(1, 1, 0), nL(1, 1, 13) },
        new [] { nL(1, 0, 12), nL(0, 0, 0), nL(0, 0, 1), nL(0, 0, 0), nL(1, 0, 4), nL(0, 0, 0), nL(0, 0, 0), nL(0, 0, 0), nL(0, 0, 0), nL(0, 0, 0), nL(1, 1, 14) },
        new [] { nL(1, 0, 11), nL(0, 0, 0), nL(0, 0, 0), nL(0, 0, 0), nL(1, 0, 5), nL(0, 0, 0), nL(0, 0, 0), nL(0, 0, 0), nL(0, 0, 0), nL(0, 0, 0), nL(1, 1, 15) },
        new [] { nL(1, 0, 10), nL(1, 0, 9), nL(1, 0, 8), nL(1, 0, 7), nL(1, 0, 6), nL(1, 1, 20), nL(1, 1, 19), nL(1, 1, 18), nL(1, 1, 17), nL(1, 1, 16), nL(0, 0, 0) }
    };

    public static readonly int cellHeight = 0;
    //(new Enterprise(0, new Industry(new List<Position>(), "", Color.black), "", null)).TextToPrintInAField.Length;
    public static readonly int maxCellWidth = 15;

    private static List<int> nL(params int[] nums) {
        List<int> list = new List<int>();
        foreach (var num in nums) {
            list.Add(num);
        }
        return list;
    }

    public static bool IsNotBoard(List<int> indexes) {
        return indexes[0] == 0;
    }
    public static string[] GetCellText(Field field, List<int> indexes) {
        string[] res = new string[cellHeight];
        if (indexes[0] == 1) {
            string[] cardText = new string[1];
            if (indexes[1] == -1) {
                //cardText = field.startCell.TextToPrintInAField;
            }
            else {
                //cardText = field.fieldArrays[indexes[1]][indexes[2]].TextToPrintInAField;
            }
            
            int emptyStrAboveAmount = (cellHeight - cardText.Length) / 2;
            int emptyStrBelowAmount = cellHeight - (cardText.Length + emptyStrAboveAmount);

            int index = 0;
            for (int k = 0; k < emptyStrAboveAmount; k++, index++) {
                res[index] = new string(' ', maxCellWidth);
            }
            for (int k = 0; k < cardText.Length; k++, index++) {
                int spacesLeftAmount = (maxCellWidth - cardText[k].Length) / 2;
                int spacesRightAmount = maxCellWidth - (cardText[k].Length + spacesLeftAmount);
                res[index] = new string(' ', spacesLeftAmount) + cardText[k] + new string(' ', spacesRightAmount);
            }
            for (int k = 0; k < emptyStrBelowAmount; k++, index++) {
                res[index] = new string(' ', maxCellWidth);
            }
        }
        else {
            if (indexes[2] == 0) {
                for (int i = 0; i < cellHeight; i++) {
                    res[i] = new string(' ', maxCellWidth);
                }
            }
            else if (indexes[2] == 1) {
                for (int i = 0; i < cellHeight - 1; i++) {
                    res[i] = new string(' ', maxCellWidth);
                }

                string curCountryStr = field.countriesArray[indexes[1]];
                int spacesLeftAmount = (maxCellWidth - curCountryStr.Length) / 2;
                int spacesRightAmount = maxCellWidth - (curCountryStr.Length + spacesLeftAmount);
                res[cellHeight - 1] = new string(' ', spacesLeftAmount) + curCountryStr + new string(' ', spacesRightAmount);
            }
        }

        return res;
    }
    
    public static string PrintCellTitleInAText(Card? card) {
        return (card is Enterprise enterprise) ? (enterprise.title) : "u";
        //(MakeOneStringFromArray(card.TextToPrintInAField));
    }

    public static string GetCountryNameByPlayer(Field field, Player player) {
        string ans;
        if (player.positionInField == null) {
            ans = "місці, де пролягає кордон двох країн";
        }
        else if (player.positionInField.cellIndex > Field.specialIndexesByCellNames["ExitChance"]) {
            ans = "міжкраїнному просторі";
        }
        else {
            ans = field.countriesArray[player.positionInField.arrayIndex];
        }

        return ans;
    }

    public static string CurWideLine(int curEnterprisesAmount, bool withSideBoards) {
        string strToReturn = "";
        string strToDuplicate = "";

        strToDuplicate += withSideBoards ? '|' : ' ';
        strToDuplicate += new string('_', JustOutput.maxSymbolsInOneCell);
        strToDuplicate += withSideBoards ? '|' : ' ';
        strToDuplicate += ' ';

        for (int i = 0; i < curEnterprisesAmount; i++) {
            strToReturn += strToDuplicate;
        }

        return strToReturn;
    }

    public static string MakeOneStringFromArray(string[] array) {
        string strToReturn = "";
        foreach(var str in array) {
            strToReturn += str + " ";
        }
        strToReturn = strToReturn.Remove(strToReturn.Length - 1, 1);
        return strToReturn;
    }

    //__________________________________________________________________________________________________________________________________________
    // Endings

    public static string TextHryvnaEnding(int moneyAmount) {
        moneyAmount %= 10;
        return moneyAmount == 1 ? "гривня" : moneyAmount is > 1 and < 5 ? "гривні" : "гривень";
    }

    public static string TextDayEnding(int daysAmount) {
        daysAmount %= 10;
        return daysAmount == 1 ? "день" : daysAmount is > 1 and < 5 ? "дні" : "днів";
    }

    public static string TurnEnding(int turnsAmount) {
        turnsAmount %= 10;
        return (turnsAmount == 1) ? "хід" : ((turnsAmount is > 1 and < 5) ? "ходи" : "ходів");
    }
}
