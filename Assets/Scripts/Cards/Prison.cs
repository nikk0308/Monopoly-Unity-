public class Prison : Card
{
    private const int startTurnsToGoOut = 3;
    private const int startPriceToPay = 100;
    private const int additionPriceForEachTurn = 50;

    public override void DoActionIfArrived(Field field, Player player, out bool isUnfinishedMethod, ref string text1, ref string text2) {
        SendPlayerToPrison(player, out isUnfinishedMethod, ref text1, ref text2);
    }

    public override void DoActionIfStayed(Field field, Player player, out bool isNextMoveNeed, out bool isUnfinishedMethod, 
        ref string text1, ref string text2) {
        CanPlayerGoOut(player, out isNextMoveNeed, out isUnfinishedMethod, ref text1, ref text2);
    }
    public override void DoActionIfArrivedAndUnfinished(Field field, Player player, bool yesOrNo, ref string text1, ref string text2) {
    }

    public override void DoActionIfStayedAndUnfinished(Field field, Player player, bool yesOrNo, out bool isNextMoveNeed, 
        ref string text1, ref string text2) {
        CanPlayerGoOutContinue(player, yesOrNo, out isNextMoveNeed, ref text1, ref text2);
    }

    private void CanPlayerGoOut(Player player, out bool isNextMoveNeed, out bool isUnfinishedMethod, ref string text1, ref string text2) {
        string curStrShow;
        isNextMoveNeed = false;
        isUnfinishedMethod = false;

        player.turnsToGoOutOfPrison--;
        int turnsLeft = player.turnsToGoOutOfPrison;
        if (turnsLeft == 0) {
            curStrShow = OutputPhrases.TextGoOutOfPrisonOrNot(player, true);
            isNextMoveNeed = true;
        }
        else {
            if (!IsEnoughMoney(player, turnsLeft)) {
                curStrShow = OutputPhrases.TextBuyFreedomOrNot(player, false) + "\n" +
                             OutputPhrases.TextTurnsRemaining(player, player.turnsToGoOutOfPrison);
            }
            else {
                if (player.IsABot()) {
                    bool botChoice = player.PayToGoOutOfPrisonOrNotBot();
                    CanPlayerGoOutContinue(player, botChoice, out isNextMoveNeed, ref text1, ref text2);
                    return;
                }
                isUnfinishedMethod = true;
                int priceToPay = (startPriceToPay + turnsLeft * additionPriceForEachTurn) *
                    (player.howManyTimesPayedInPrison + 1);
                curStrShow = OutputPhrases.TextIsPayedForFreedom(priceToPay);
            }
        }
        GameShowManager.Instance.FieldToShow.AutoAddText(ref text1, ref text2, curStrShow);
    }

    private void CanPlayerGoOutContinue(Player player, bool yesOrNo, out bool isNextMoveNeed, ref string text1, ref string text2) {
        isNextMoveNeed = false;
        string curStrShow;
        int priceToPay = (startPriceToPay + player.turnsToGoOutOfPrison * additionPriceForEachTurn) *
                         (player.howManyTimesPayedInPrison + 1);
        if (yesOrNo) {
            player.howManyTimesPayedInPrison++;
            player.moneyAmount -= priceToPay;
            player.turnsToGoOutOfPrison = 0;
            isNextMoveNeed = true;
            
            curStrShow = OutputPhrases.TextBuyFreedomOrNot(player, true);
            GameShowManager.Instance.FieldToShow.AutoAddText(ref text1, ref text2, curStrShow);
            return;
        }
        curStrShow = OutputPhrases.TextGoOutOfPrisonOrNot(player, false);
        GameShowManager.Instance.FieldToShow.AutoAddText(ref text1, ref text2, curStrShow);
    }

    private bool IsEnoughMoney(Player player, int turnsToGoOut) {
        int priceToPay = (startPriceToPay + turnsToGoOut * additionPriceForEachTurn) *
                         (player.howManyTimesPayedInPrison + 1);
        return player.moneyAmount >= priceToPay;
    }

    private void SendPlayerToPrison(Player player, out bool isUnfinishedMethod, ref string text1, ref string text2) {
        isUnfinishedMethod = false;
        player.turnsToGoOutOfPrison = startTurnsToGoOut;
        string curStrShow = OutputPhrases.TextSendPlayerToPrison(player, startTurnsToGoOut);
        GameShowManager.Instance.FieldToShow.AutoAddText(ref text1, ref text2, curStrShow);
    }
}
