public class Bonus : Card
{
    private delegate void Action(Field field, Player player, out bool isUnfinishedMethod, ref string text1, ref string text2);

    private readonly int[] probability = { 35, 15, 10, 30, 10 };

    public override void DoActionIfArrived(Field field, Player player, out bool isUnfinishedMethod, ref string text1, ref string text2) {
        GivePlayerABonus(field, player, out isUnfinishedMethod, ref text1, ref text2);
    }

    public override void DoActionIfStayed(Field field, Player player, out bool isNextMoveNeed, out bool isUnfinishedMethod, ref string text1, ref string text2) {
        JustTurn(player, out isNextMoveNeed, out isUnfinishedMethod, ref text1, ref text2);
    }
    public override void DoActionIfArrivedAndUnfinished(Field field, Player player, bool yesOrNo, ref string text1, ref string text2) {
    }
    public override void DoActionIfStayedAndUnfinished(Field field, Player player, bool yesOrNo, out bool isNextMoveNeed, ref string text1, ref string text2) {
        isNextMoveNeed = false;
    }


    private void GivePlayerABonus(Field field, Player player, out bool isUnfinishedMethod, ref string text1, ref string text2) {
        Action action = Choose();
        isUnfinishedMethod = false;
        action(field, player, out isUnfinishedMethod, ref text1, ref text2);
    }

    private Action Choose() {
        return GetActionNumber() switch {
            0 => Give100ToPlayer,
            1 => Give200ToPlayer,
            2 => Give500ToPlayer,
            3 => MovePlayerToStart,
            _ => GiveNothing
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

    private void Give100ToPlayer(Field field, Player player, out bool isUnfinishedMethod, ref string text1, ref string text2) {
        isUnfinishedMethod = false;
        int bonusMoney = 100;
        player.moneyAmount += bonusMoney;
        string curStrShow = OutputPhrases.TextBonusOrZradaMoneyAmount(player, bonusMoney, true);
        GameShowManager.Instance.FieldToShow.AutoAddText(ref text1, ref text2, curStrShow);
    }

    private void Give200ToPlayer(Field field, Player player, out bool isUnfinishedMethod, ref string text1, ref string text2) {
        isUnfinishedMethod = false;
        int bonusMoney = 200;
        player.moneyAmount += bonusMoney;
        string curStrShow = OutputPhrases.TextBonusOrZradaMoneyAmount(player, bonusMoney, true);
        GameShowManager.Instance.FieldToShow.AutoAddText(ref text1, ref text2, curStrShow);
    }

    private void Give500ToPlayer(Field field, Player player, out bool isUnfinishedMethod, ref string text1, ref string text2) {
        isUnfinishedMethod = false;
        int bonusMoney = 500;
        player.moneyAmount += bonusMoney;
        string curStrShow = OutputPhrases.TextBonusOrZradaMoneyAmount(player, bonusMoney, true);
        GameShowManager.Instance.FieldToShow.AutoAddText(ref text1, ref text2, curStrShow);
    }

    private void MovePlayerToStart(Field field, Player player, out bool isUnfinishedMethod, ref string text1, ref string text2) {
        player.positionInField = null;
        string curStrShow = OutputPhrases.ZradaOrBonusMovedTo(player, true);
        GameShowManager.Instance.FieldToShow.AutoAddText(ref text1, ref text2, curStrShow);
        field.startCell.DoActionIfArrived(field, player, out isUnfinishedMethod, ref text1, ref text2);
    }

    private void GiveNothing(Field field, Player player, out bool isUnfinishedMethod, ref string text1, ref string text2) {
        isUnfinishedMethod = false;
        string curStrShow = OutputPhrases.TextNoGainOrTake(player);
        GameShowManager.Instance.FieldToShow.AutoAddText(ref text1, ref text2, curStrShow);
    }
}
