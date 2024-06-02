using UnityEngine;
public abstract class Card
{
    public abstract void DoActionIfArrived(Field field, Player player, out bool isUnfinishedMethod, ref string text1, ref string text2);
    public abstract void DoActionIfStayed(Field field, Player player, out bool isNextMoveNeed, out bool isUnfinishedMethod, ref string text1, ref string text2);
    public abstract void DoActionIfArrivedAndUnfinished(Field field, Player player, bool yesOrNo, ref string text1, ref string text2);
    public abstract void DoActionIfStayedAndUnfinished(Field field, Player player, bool yesOrNo, out bool isNextMoveNeed, ref string text1, ref string text2);

    private protected void JustTurn(Player player, out bool isNextMoveNeed, out bool isUnfinishedMethod, ref string text1, ref string text2) {
        isNextMoveNeed = true;
        isUnfinishedMethod = false;
        string curStrShow = OutputPhrases.PlayerTurns(player);
        GameShowManager.Instance.FieldToShow.AutoAddText(ref text1, ref text2, curStrShow);
    }
}
