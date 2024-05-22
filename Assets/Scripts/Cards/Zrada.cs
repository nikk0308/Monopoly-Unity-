public class Zrada : Card
{
    private delegate string Action(Field field, Player player);

    private readonly int[] probability = { 35, 15, 10, 30, 10 };

    public override string[] TextToPrintInAField {
        get { return OutputPhrases.outputTextByTags["Zrada"]; }
    }

    public override string DoActionIfArrived(Field field, Player player) {
        return GivePlayerAZrada(field, player);
    }

    public override string DoActionIfStayed(Field field, Player player, out bool isNextMoveNeed) {
        return JustTurn(player, out isNextMoveNeed);
    }

    private string GivePlayerAZrada(Field field, Player player) {
        Action action = Choose();
        return action(field, player);
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

    private string Take100FromAPlayer(Field field, Player player) {
        int moneyToTake = 100;
        player.moneyAmount -= moneyToTake;
        return OutputPhrases.TextBonusOrZradaMoneyAmount(player, moneyToTake, false);
    }

    private string Take200FromAPlayer(Field field, Player player) {
        int moneyToTake = 200;
        player.moneyAmount -= moneyToTake;
        return OutputPhrases.TextBonusOrZradaMoneyAmount(player, moneyToTake, false);
    }

    private string Take500FromAPlayer(Field field, Player player) {
        int moneyToTake = 500;
        player.moneyAmount -= moneyToTake;
        return OutputPhrases.TextBonusOrZradaMoneyAmount(player, moneyToTake, false);
    }

    private string MovePlayerToPrison(Field field, Player player) {
        int prisonIndex = field.specialIndexesByCellNames["Prison"];
        player.positionInField.cellIndex = prisonIndex;
        return field.fieldArrays[player.positionInField.arrayIndex][prisonIndex].DoActionIfArrived(field, player);
    }

    private string Take0FromAPlayer(Field field, Player player) {
        return OutputPhrases.TextNoGainOrTake(player);
    }
}
