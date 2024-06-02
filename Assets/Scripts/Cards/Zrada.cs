using UnityEngine;

public class Zrada : Card
{
    private delegate void Action(Field field, Player player, out bool isUnfinishedMethod, ref string text1, ref string text2);

    private readonly int[] probability = { 35, 15, 10, 30, 10 };

    public override void DoActionIfArrived(Field field, Player player, out bool isUnfinishedMethod, ref string text1, ref string text2) {
        GivePlayerAZrada(field, player, out isUnfinishedMethod, ref text1, ref text2);
    }

    public override void DoActionIfStayed(Field field, Player player, out bool isNextMoveNeed, out bool isUnfinishedMethod, 
        ref string text1, ref string text2) {
        JustTurn(player, out isNextMoveNeed, out isUnfinishedMethod, ref text1, ref text2);
    }

    public override void DoActionIfArrivedAndUnfinished(Field field, Player player, bool yesOrNo, ref string text1, ref string text2) {
    }
    public override void DoActionIfStayedAndUnfinished(Field field, Player player, bool yesOrNo, out bool isNextMoveNeed, 
        ref string text1, ref string text2) {
        isNextMoveNeed = false;
    }

    private void GivePlayerAZrada(Field field, Player player, out bool isUnfinishedMethod, ref string text1, ref string text2) {
        isUnfinishedMethod = false;
        Action action = Choose();
        action(field, player, out isUnfinishedMethod, ref text1, ref text2);
    }

    private Action Choose() {
        return GetActionNumber() switch {
            0 => Take100FromAPlayer,
            1 => Take200FromAPlayer,
            2 => Take500FromAPlayer,
            3 => MovePlayerToPrison,
            _ => Take0FromAPlayer
        };
    }

    private int GetActionNumber() {
        int randFrom0To100 = Constants.Rand.Next(100);
        int curIndex = -1;
        do {
            curIndex++;
            randFrom0To100 -= probability[curIndex];
        } while (randFrom0To100 >= 0);

        return curIndex;
    }

    private void Take100FromAPlayer(Field field, Player player, out bool isUnfinishedMethod, ref string text1, ref string text2) {
        isUnfinishedMethod = false;
        int moneyToTake = 100;
        player.moneyAmount -= moneyToTake;
        string curStrShow = OutputPhrases.TextBonusOrZradaMoneyAmount(player, moneyToTake, false);
        GameShowManager.Instance.FieldToShow.AutoAddText(ref text1, ref text2, curStrShow);
    }

    private void Take200FromAPlayer(Field field, Player player, out bool isUnfinishedMethod, ref string text1, ref string text2) {
        isUnfinishedMethod = false;
        int moneyToTake = 200;
        player.moneyAmount -= moneyToTake;
        string curStrShow = OutputPhrases.TextBonusOrZradaMoneyAmount(player, moneyToTake, false);
        GameShowManager.Instance.FieldToShow.AutoAddText(ref text1, ref text2, curStrShow);
    }

    private void Take500FromAPlayer(Field field, Player player, out bool isUnfinishedMethod, ref string text1, ref string text2) {
        isUnfinishedMethod = false;
        int moneyToTake = 500;
        player.moneyAmount -= moneyToTake;
        string curStrShow = OutputPhrases.TextBonusOrZradaMoneyAmount(player, moneyToTake, false);
        GameShowManager.Instance.FieldToShow.AutoAddText(ref text1, ref text2, curStrShow);
    }

    private void MovePlayerToPrison(Field field, Player player, out bool isUnfinishedMethod, ref string text1, ref string text2) {
        int prisonIndex = Field.specialIndexesByCellNames["Prison"];
        player.positionInField.cellIndex = prisonIndex;
        string curStrShow = OutputPhrases.ZradaOrBonusMovedTo(player, false);
        GameShowManager.Instance.FieldToShow.AutoAddText(ref text1, ref text2, curStrShow);
        field.fieldArrays[player.positionInField.arrayIndex][prisonIndex].DoActionIfArrived(field, player, out isUnfinishedMethod, ref text1, ref text2);
    }

    private void Take0FromAPlayer(Field field, Player player, out bool isUnfinishedMethod, ref string text1, ref string text2) {
        isUnfinishedMethod = false;
        string curStrShow = OutputPhrases.TextNoGainOrTake(player);
        GameShowManager.Instance.FieldToShow.AutoAddText(ref text1, ref text2, curStrShow);
    }
}
