public class Prison : Card
{
    private const int startTurnsToGoOut = 3;
    private const int startPriceToPay = 100;
    private const int additionPriceForEachTurn = 50;


    public override string[] TextToPrintInAField {
        get { return OutputPhrases.outputTextByTags["Prison"]; }
    }

    public override string DoActionIfArrived(Field field, Player player) {
        return SendPlayerToPrison(player);
    }

    public override string DoActionIfStayed(Field field, Player player, out bool isNextMoveNeed) {
        return CanPlayerGoOut(player, out isNextMoveNeed);
    }

    public bool IsPayedForFreedom(Player player, int turnsToGoOut) {
        int priceToPay = (startPriceToPay + turnsToGoOut * additionPriceForEachTurn) *
                         (player.howManyTimesPayedInPrison + 1);

        JustOutput.PrintText(OutputPhrases.TextIsPayedForFreedom(priceToPay));
        string choice = player.PayToGoOutOfPrisonOrNot();

        if (choice == "1") {
            if (player.moneyAmount >= priceToPay) {
                player.howManyTimesPayedInPrison++;
                player.moneyAmount -= priceToPay;
                return true;
            }

            JustOutput.PrintText(OutputPhrases.TextBuyFreedomOrNot(player, false));
            return false;
        }

        JustOutput.PrintText(OutputPhrases.TextGoOutOfPrisonOrNot(player, false));
        return false;
    }

    private string CanPlayerGoOut(Player player, out bool isNextMoveNeed) {
        string msgToReturn;
        isNextMoveNeed = false;

        player.turnsToGoOutOfPrison--;
        int turnsLeft = player.turnsToGoOutOfPrison;
        if (turnsLeft == 0) {
            msgToReturn = OutputPhrases.TextGoOutOfPrisonOrNot(player, true);
            isNextMoveNeed = true;
        }
        else {
            if (IsPayedForFreedom(player, turnsLeft)) {
                player.turnsToGoOutOfPrison = 0;
                msgToReturn = OutputPhrases.TextBuyFreedomOrNot(player, true);
                isNextMoveNeed = true;
            }
            else {
                msgToReturn = OutputPhrases.TextTurnsRemaining(player, player.turnsToGoOutOfPrison);
            }
        }

        return msgToReturn;
    }

    private string SendPlayerToPrison(Player player) {
        player.turnsToGoOutOfPrison = startTurnsToGoOut;
        return OutputPhrases.TextSendPlayerToPrison(player, startTurnsToGoOut);
    }
}
