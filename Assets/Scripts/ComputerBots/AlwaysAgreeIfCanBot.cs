using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysAgreeIfCanBot : AIBot
{
    public string BotBuyEnterpriseOrNot(Player player, Enterprise enterprise) {
        string choice = "1";
        JustOutput.PrintText(choice);
        return choice;
    }

    public string BotPayToGoOutOfPrisonOrNot(Player player) {
        string choice = "1";
        JustOutput.PrintText(choice);
        return choice;
    }

    public string BotStayOnWorkOrNot(Player player) {
        string choice = "2";
        JustOutput.PrintText(choice);
        return choice;
    }

    public int BotWhichEnterprisePawnToNotLose(Player player, List<Enterprise> enterprises) {
        int choice = 1;
        JustOutput.PrintText(Convert.ToString(choice));
        return choice;
    }

    public string BotPawnEnterpriseOrBuildHotelPreTurn(Player player, List<Enterprise> notPawnedEnterprises,
        List<Enterprise> pawnedEnterprises, List<Enterprise> enterprisesToBuildHotel) {
        string choice;
        if (pawnedEnterprises.Count == 0) {
            choice = "0";
        }
        else {
            choice = "2";
        }
        JustOutput.PrintText(choice);
        return choice;
    }

    public int BotWhichEnterprisePawnPreTurn(Player player, List<Enterprise> notPawnedEnterprises) { // Will be never used
        int choice = 1;
        JustOutput.PrintText(Convert.ToString(choice));
        return choice;
    }
    
    public int BotWhichEnterpriseUnPawnPreTurn(Player player, List<Enterprise> pawnedEnterprises) {
        int choice = 1;
        JustOutput.PrintText(Convert.ToString(choice));
        return choice;
    }

    public int BotWhichEnterpriseBuildHotelPreTurn(Player player, List<Enterprise> enterprisesToBuildHotel) { // Will be never used
        int choice = 1;
        JustOutput.PrintText(Convert.ToString(choice));
        return choice;
    }
}
