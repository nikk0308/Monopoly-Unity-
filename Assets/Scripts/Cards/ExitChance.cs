using UnityEngine;

public class ExitChance : Card
{
    public override void DoActionIfArrived(Field field, Player player, out bool isUnfinishedMethod, ref string text1, ref string text2) {
        GuessIsGoOut(player, out isUnfinishedMethod, ref text1, ref text2);
    }

    public override void DoActionIfStayed(Field field, Player player, out bool isNextMoveNeed, out bool isUnfinishedMethod, 
        ref string text1, ref string text2) {
        GoOutOrNot(player, out isNextMoveNeed, out isUnfinishedMethod, ref text1, ref text2);
    }
    public override void DoActionIfArrivedAndUnfinished(Field field, Player player, bool yesOrNo, ref string text1, ref string text2) {
    }

    public override void DoActionIfStayedAndUnfinished(Field field, Player player, bool yesOrNo, out bool isNextMoveNeed, 
        ref string text1, ref string text2) {
        isNextMoveNeed = false;
    }

    private void GoOutOrNot(Player player, out bool isNextMoveNeed, out bool isUnfinishedMethod, ref string text1, ref string text2) {
        isNextMoveNeed = true;
        isUnfinishedMethod = false;
        bool isGoOut = player.canGoOutOfCountry;

        if (!player.canGoOutOfCountry) {
            player.positionInField.cellIndex = -1;
        }
        player.canGoOutOfCountry = false;
        
        string curStrShow = OutputPhrases.TextGoOutOrNot(player, isGoOut);
        GameShowManager.Instance.FieldToShow.AutoAddText(ref text1, ref text2, curStrShow);
    }

    private void GuessIsGoOut(Player player, out bool isUnfinishedMethod, ref string text1, ref string text2) {
        isUnfinishedMethod = false;
        bool isGoOut = Constants.RollCoin(80, 20);
        player.canGoOutOfCountry = isGoOut;
        
        string curStrShow = OutputPhrases.TextGuessIsGoOut(player);
        GameShowManager.Instance.FieldToShow.AutoAddText(ref text1, ref text2, curStrShow);
    }
}
