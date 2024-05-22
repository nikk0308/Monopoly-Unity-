public class Bonus : Card
{
    private delegate string Action(Field field, Player player);

    private readonly int[] probability = { 35, 15, 10, 30, 10 };

    public override string[] TextToPrintInAField {
        get { return OutputPhrases.outputTextByTags["Bonus"]; }
    }

    public override string DoActionIfArrived(Field field, Player player) {
        return GivePlayerABonus(field, player);
    }

    public override string DoActionIfStayed(Field field, Player player, out bool isNextMoveNeed) {
        return JustTurn(player, out isNextMoveNeed);
    }

    private string GivePlayerABonus(Field field, Player player) {
        Action action = Choose();
        return action(field, player);
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

    private string Give100ToPlayer(Field field, Player player) {
        int bonusMoney = 100;
        player.moneyAmount += bonusMoney;
        return OutputPhrases.TextBonusOrZradaMoneyAmount(player, bonusMoney, true);
    }

    private string Give200ToPlayer(Field field, Player player) {
        int bonusMoney = 200;
        player.moneyAmount += bonusMoney;
        return OutputPhrases.TextBonusOrZradaMoneyAmount(player, bonusMoney, true);
    }

    private string Give500ToPlayer(Field field, Player player) {
        int bonusMoney = 500;
        player.moneyAmount += bonusMoney;
        return OutputPhrases.TextBonusOrZradaMoneyAmount(player, bonusMoney, true);
    }

    private string MovePlayerToStart(Field field, Player player) {
        player.positionInField = null;
        return field.startCell.DoActionIfArrived(field, player);
    }

    private string GiveNothing(Field field, Player player) {
        return OutputPhrases.TextNoGainOrTake(player);
    }
}
