public class ExitChance : Card
{
    public override string[] TextToPrintInAField {
        get { return OutputPhrases.outputTextByTags["ExitChance"]; } 
    }
    public override string DoActionIfArrived(Field field, Player player) {
        return GuessIsGoOut(player);
    }

    public override string DoActionIfStayed(Field field, Player player, out bool isNextMoveNeed) {
        return GoOutOrNot(player, out isNextMoveNeed);
    }

    private string GoOutOrNot(Player player, out bool isNextMoveNeed) {
        isNextMoveNeed = true;
        bool isGoOut = player.canGoOutOfCountry;

        if (!player.canGoOutOfCountry) {
            player.positionInField.cellIndex = -1;
        }
        player.canGoOutOfCountry = false;
        
        return OutputPhrases.TextGoOutOrNot(player, isGoOut);
    }

    private string GuessIsGoOut(Player player) {
        bool isGoOut = Constants.RollCoin(80, 20);
        player.canGoOutOfCountry = isGoOut;
        return OutputPhrases.TextGuessIsGoOut(player);
    }
}
