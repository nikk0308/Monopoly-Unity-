public class Review : Card
{

    public override void DoActionIfArrived(Field field, Player player, out bool isUnfinishedMethod, ref string text1, ref string text2) {
        GoToPrisonOrNot(field, player, out isUnfinishedMethod, ref text1, ref text2);
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

    private void GoToPrisonOrNot(Field field, Player player, out bool isUnfinishedMethod, ref string text1, ref string text2) {
        isUnfinishedMethod = false;
        string curStrShow;
        bool isGoToPrison = Constants.RollCoin(20, 80);

        if (!isGoToPrison) {
            curStrShow = OutputPhrases.TextGoToPrisonOrNot(player, isGoToPrison);
            GameShowManager.Instance.FieldToShow.AutoAddText(ref text1, ref text2, curStrShow);
            return;
        }

        int prisonIndex = Field.specialIndexesByCellNames["Prison"];
        player.positionInField.cellIndex = prisonIndex;
        curStrShow = OutputPhrases.TextGoToPrisonOrNot(player, isGoToPrison);
        GameShowManager.Instance.FieldToShow.AutoAddText(ref text1, ref text2, curStrShow);
        
        field.fieldArrays[player.positionInField.arrayIndex][prisonIndex]
            .DoActionIfArrived(field, player, out isUnfinishedMethod, ref text1, ref text2);
    }
}
