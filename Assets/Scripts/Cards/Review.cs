public class Review : Card
{
    public override string[] TextToPrintInAField {
        get { return OutputPhrases.outputTextByTags["Review"]; }
    }

    public override string DoActionIfArrived(Field field, Player player) {
        return GoToPrisonOrNot(field, player);
    }

    public override string DoActionIfStayed(Field field, Player player, out bool isNextMoveNeed) {
        return JustTurn(player, out isNextMoveNeed);
    }

    private string GoToPrisonOrNot(Field field, Player player) {
        bool isGoToPrison = Constants.RollCoin(20, 80);

        if (!isGoToPrison) {
            return OutputPhrases.TextGoToPrisonOrNot(player, isGoToPrison);
        }

        int prisonIndex = field.specialIndexesByCellNames["Prison"];
        player.positionInField.cellIndex = prisonIndex;
        field.fieldArrays[player.positionInField.arrayIndex][prisonIndex].DoActionIfArrived(field, player);
        return OutputPhrases.TextGoToPrisonOrNot(player, isGoToPrison);
    }
}
