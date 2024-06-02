using UnityEngine;

public class Work : Card
{
    private const int startWorkingTerm = 3;
    private const int lowerSalaryBoard = 30;
    private const int upperSalaryBoard = 150;

    public override void DoActionIfArrived(Field field, Player player, out bool isUnfinishedMethod, ref string text1, ref string text2) {
        StartWork(player, out isUnfinishedMethod, ref text1, ref text2);
    }

    public override void DoActionIfStayed(Field field, Player player, out bool isNextMoveNeed, out bool isUnfinishedMethod, 
        ref string text1, ref string text2) {
        Working(player, out isNextMoveNeed, out isUnfinishedMethod, ref text1, ref text2);
    }
    public override void DoActionIfArrivedAndUnfinished(Field field, Player player, bool yesOrNo, ref string text1, ref string text2) {
    }

    public override void DoActionIfStayedAndUnfinished(Field field, Player player, bool yesOrNo, out bool isNextMoveNeed, 
        ref string text1, ref string text2) {
        WorkingContinue(player, yesOrNo, out isNextMoveNeed, ref text1, ref text2);
    }

    private void StartWork(Player player, out bool isUnfinishedMethod, ref string text1, ref string text2) {
        isUnfinishedMethod = false;
        string curStrShow;
        player.turnsCanContinueWork = startWorkingTerm - player.howManyTimesWorked;

        if (player.turnsCanContinueWork == 0) {
            curStrShow = OutputPhrases.TextStartWork(player, false);
            GameShowManager.Instance.FieldToShow.AutoAddText(ref text1, ref text2, curStrShow);
            return;
        }
        curStrShow = OutputPhrases.TextStartWork(player, true);
        GameShowManager.Instance.FieldToShow.AutoAddText(ref text1, ref text2, curStrShow);
    }

    private void Working(Player player, out bool isNextMoveNeed, out bool isUnfinishedMethod, ref string text1, ref string text2) {
        isUnfinishedMethod = false;
        isNextMoveNeed = false;
        string curStrShow;
        if (player.turnsCanContinueWork == 0) {
            isNextMoveNeed = true;
            if (player.howManyTimesWorked < startWorkingTerm) {
                player.howManyTimesWorked++;
            }

            curStrShow = OutputPhrases.TextFinishWorking(player, true);
            GameShowManager.Instance.FieldToShow.AutoAddText(ref text1, ref text2, curStrShow);
            return;
        }
        
        if (player.IsABot()) {
            bool botChoice = player.StayOnWorkOrNotBot();
            WorkingContinue(player, botChoice, out isNextMoveNeed, ref text1, ref text2);
            return;
        }
        
        isUnfinishedMethod = true;
        curStrShow = OutputPhrases.TextWorkChoice(player);
        GameShowManager.Instance.FieldToShow.AutoAddText(ref text1, ref text2, curStrShow);
    }

    private void WorkingContinue(Player player, bool yesOrNo, out bool isNextMoveNeed, ref string text1, ref string text2) {
        string curStrShow;
        if (yesOrNo) {
            isNextMoveNeed = false;
            player.turnsCanContinueWork--;

            int randSalary = Constants.Rand.Next(lowerSalaryBoard, upperSalaryBoard + 1) * (player.howManyTimesWorked + 1);
            randSalary /= 10;
            randSalary *= 10;
            player.moneyAmount += randSalary;
            
            curStrShow = OutputPhrases.TextBossPayed(player, randSalary);
            GameShowManager.Instance.FieldToShow.AutoAddText(ref text1, ref text2, curStrShow);
            return;
        }

        isNextMoveNeed = true;
        player.turnsCanContinueWork = 0;
        if (player.howManyTimesWorked < startWorkingTerm) {
            player.howManyTimesWorked++;
        }

        curStrShow = OutputPhrases.TextFinishWorking(player, false);
        GameShowManager.Instance.FieldToShow.AutoAddText(ref text1, ref text2, curStrShow);
    }
}
