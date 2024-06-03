using System.Collections.Generic;
using UnityEngine;

public class OutputPhrases : MonoBehaviour
{
    
    //__________________________________________________________________________________________________________________________________________
    // CARDS

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

    public static string TextStartTurn(Player player, Field field, int startMoney) {
        return player.nameInGame + " випадає доля йти в країну " +
               Constants.countryNamesByCountryNames[field.countriesArray[player.positionInField.arrayIndex]] +
               ". Видано стартові " + startMoney + " гривень на підняття економіки країни";
    }

    public static string TextGainSalary(Player player, int salaryAmount) {
        return "Гравець " + player.nameInGame + " забігає швиденько на роботу, отримує премію в розмірі " + salaryAmount + " гривень";
    }
    
    public static string PlayerTurns(Player player) {
        return player.nameInGame + " ходить.";
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
