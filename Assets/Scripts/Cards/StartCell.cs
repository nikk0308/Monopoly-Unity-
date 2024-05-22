using System;

public class StartCell : Card
{
    public int enterTnArrayAfterStart;

    public override string[] TextToPrintInAField {
        get { return OutputPhrases.outputTextByTags["Start"]; }
    }

    public override string DoActionIfArrived(Field field, Player player) {
        return OutputPhrases.MovedToStart(player);
    }

    public override string DoActionIfStayed(Field field, Player player, out bool isNextMoveNeed) {
        return StartTurn(field, player, out isNextMoveNeed);
    }

    private string StartTurn(Field field, Player player, out bool isNextMoveNeed) {
        isNextMoveNeed = true;
        enterTnArrayAfterStart = field.specialIndexesByCellNames["Bonus"] + 1;

        player.positionInField = new Position();
        player.positionInField.cellIndex = enterTnArrayAfterStart - 1;

        int countryIndex = Convert.ToInt32(Constants.RollCoin(50, 50));
        player.positionInField.arrayIndex = countryIndex;
        player.moneyAmount += GamePlay.startCapital;
        return OutputPhrases.TextStartTurn(player, field, GamePlay.startCapital);
    }
}
