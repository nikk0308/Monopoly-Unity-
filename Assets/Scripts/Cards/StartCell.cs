using System;
using UnityEngine;

public class StartCell : Card
{
    public int enterTnArrayAfterStart;

    public override void DoActionIfArrived(Field field, Player player, out bool isUnfinishedMethod, ref string text1, ref string text2) {
        isUnfinishedMethod = false;
        string curStrShow = OutputPhrases.MovedToStart(player);
        GameShowManager.Instance.FieldToShow.AutoAddText(ref text1, ref text2, curStrShow);
    }

    public override void DoActionIfStayed(Field field, Player player, out bool isNextMoveNeed, out bool isUnfinishedMethod, 
        ref string text1, ref string text2) {
        StartTurn(field, player, out isNextMoveNeed, out isUnfinishedMethod, ref text1, ref text2);
    }
    public override void DoActionIfArrivedAndUnfinished(Field field, Player player, bool yesOrNo, ref string text1, ref string text2) {
    }

    public override void DoActionIfStayedAndUnfinished(Field field, Player player, bool yesOrNo, out bool isNextMoveNeed, 
        ref string text1, ref string text2) {
        isNextMoveNeed = false;
    }

    private void StartTurn(Field field, Player player, out bool isNextMoveNeed, out bool isUnfinishedMethod, ref string text1, ref string text2) {
        isNextMoveNeed = true;
        isUnfinishedMethod = false;
        enterTnArrayAfterStart = Field.specialIndexesByCellNames["Bonus"] + 1;
        int countryIndex = Convert.ToInt32(Constants.RollCoin(50, 50));
        player.positionInField = new Position(countryIndex, enterTnArrayAfterStart);
        
        player.moneyAmount += Constants.StartCapital;
        string curStrShow = OutputPhrases.TextStartTurn(player, field, Constants.StartCapital);
        GameShowManager.Instance.FieldToShow.AutoAddText(ref text1, ref text2, curStrShow);
    }
}
