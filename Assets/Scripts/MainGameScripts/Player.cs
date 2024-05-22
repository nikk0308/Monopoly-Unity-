using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    internal readonly string nameInGame;
    internal readonly ConsoleColor chipColor;
    internal int moneyAmount;
    internal Position? positionInField;
    internal int turnsToGoOutOfPrison;
    internal int howManyTimesPayedInPrison;
    internal bool canGoOutOfCountry;
    internal int turnsCanContinueWork;
    internal int howManyTimesWorked;
    private AIBot? playerAI;

    public Player(string nameInGame, AIBot? playerAI, ConsoleColor chipColor, int moneyAmount = 0,
        Position? positionInField = null, int turnsToGoOutOfPrison = 0, int howManyTimesPayedInPrison = 0,
        bool canGoOutOfCountry = false, int turnsCanContinueWork = 0, int howManyTimesWorked = 0) {
        this.nameInGame = nameInGame;
        this.playerAI = playerAI;
        this.moneyAmount = moneyAmount;
        this.positionInField = positionInField;
        this.chipColor = chipColor;
        this.turnsToGoOutOfPrison = turnsToGoOutOfPrison;
        this.howManyTimesPayedInPrison = howManyTimesPayedInPrison;
        this.canGoOutOfCountry = canGoOutOfCountry;
        this.turnsCanContinueWork = turnsCanContinueWork;
        this.howManyTimesWorked = howManyTimesWorked;
    }

    public List<Enterprise> GetAllPlayerEnterprises(Field field) {
        List<Enterprise> ans = new List<Enterprise>();
        foreach (var array in field.fieldArrays) {
            foreach (var card in array) {
                if (card is Enterprise enterprise && enterprise.owner == this) {
                    ans.Add(enterprise);
                }
            }
        }

        return ans;
    }

    public List<Enterprise> GetPawnedOrNotPlayerEnterprises(Field field, bool isPawnedNeed) {
        List<Enterprise> enterprises = GetAllPlayerEnterprises(field);

        for (int i = 0; i < enterprises.Count; i++) {
            Enterprise enterprise = enterprises[i];
            if (isPawnedNeed != enterprise.IsPawned()) {
                enterprises.RemoveAt(i);
                i--;
            }
        }

        return enterprises;
    }

    public List<Enterprise> GetFullIndustryWithoutNHotelsEnterprises(Field field) {
        List<Enterprise> enterprises = GetAllPlayerEnterprises(field);
        for (int i = 0; i < enterprises.Count; i++) {
            Enterprise enterprise = enterprises[i];
            if (!(enterprise.isFullIndustry && !enterprise.isBuiltHotel)) {
                enterprises.RemoveAt(i);
                i--;
            }
        }

        return enterprises;
    }

    public void MakeTurnForPawnedEnter(Field field) {
        List<Enterprise> enterprises = GetAllPlayerEnterprises(field);
        foreach (var enterprise in enterprises) {
            if (enterprise.IsPawned()) {
                enterprise.ReduceTurnsAmount();
                if (enterprise.turnsToDisappearIfPawned == 0) {
                    enterprise.ClearEnterprise();
                }
            }
        }
    }

    public void FreeAllEnterprises(Field field) {
        // Tut?
        List<Enterprise> enterprises = GetAllPlayerEnterprises(field);
        foreach (var enterprise in enterprises) {
            enterprise.ClearEnterprise();
        }
    }

    public bool IsInPrison() {
        return turnsToGoOutOfPrison != 0;
    }


    public string BuyEnterpriseOrNot(Enterprise enterprise) {
        if (IsABot()) {
            JustOutput.PrintMyChoice();
        }

        return !IsABot()
            ? Interactive.GetPersonChoice(new List<string> { "1", "2" })
            : playerAI.BotBuyEnterpriseOrNot(this, enterprise);
    }

    public string PayToGoOutOfPrisonOrNot() {
        if (IsABot()) {
            JustOutput.PrintMyChoice();
        }

        return !IsABot()
            ? Interactive.GetPersonChoice(new List<string>() { "1", "2" })
            : playerAI.BotPayToGoOutOfPrisonOrNot(this);
    }

    public string StayOnWorkOrNot() {
        if (IsABot()) {
            JustOutput.PrintMyChoice();
        }

        return !IsABot()
            ? Interactive.GetPersonChoice(new List<string>() { "1", "2" })
            : playerAI.BotStayOnWorkOrNot(this);
    }

    public int WhichEnterprisePawnToNotLose(List<Enterprise> enterprises) {
        if (IsABot()) {
            JustOutput.PrintMyChoice();
        }

        return !IsABot()
            ? Convert.ToInt32(Interactive.GetPersonChoice(JustOutput.MakeAListFromDiapasone(1, enterprises.Count))) - 1
            : playerAI.BotWhichEnterprisePawnToNotLose(this, enterprises);
    }

    public string PawnEnterpriseOrBuildHotelPreTurn(List<Enterprise> notPawnedEnterprises,
        List<Enterprise> pawnedEnterprises, List<Enterprise> enterprisesToBuildHotel) {
        if (IsABot()) {
            JustOutput.PrintMyChoice();
        }

        return !IsABot()
            ? Interactive.GetPersonChoice(JustOutput.MakeAListFromDiapasone(0, 3))
            : playerAI.BotPawnEnterpriseOrBuildHotelPreTurn(this, notPawnedEnterprises, pawnedEnterprises,
                enterprisesToBuildHotel);
    }

    public int WhichEnterprisePawnPreTurn(List<Enterprise> notPawnedEnterprises) {
        if (IsABot()) {
            JustOutput.PrintMyChoice();
        }

        return !IsABot()
            ? Convert.ToInt32(
                Interactive.GetPersonChoice(JustOutput.MakeAListFromDiapasone(1, notPawnedEnterprises.Count))) - 1
            : playerAI.BotWhichEnterprisePawnPreTurn(this, notPawnedEnterprises);
    }

    public int WhichEnterpriseUnPawnPreTurn(List<Enterprise> pawnedEnterprises) {
        if (IsABot()) {
            JustOutput.PrintMyChoice();
        }

        return !IsABot()
            ? Convert.ToInt32(
                Interactive.GetPersonChoice(JustOutput.MakeAListFromDiapasone(1, pawnedEnterprises.Count))) - 1
            : playerAI.BotWhichEnterpriseUnPawnPreTurn(this, pawnedEnterprises);
    }

    public int WhichEnterpriseBuildHotelPreTurn(List<Enterprise> enterprisesToBuildHotel) {
        if (IsABot()) {
            JustOutput.PrintMyChoice();
        }

        return !IsABot()
            ? Convert.ToInt32(
                Interactive.GetPersonChoice(JustOutput.MakeAListFromDiapasone(1, enterprisesToBuildHotel.Count))) - 1
            : playerAI.BotWhichEnterpriseBuildHotelPreTurn(this, enterprisesToBuildHotel);
    }

    public bool IsABot() {
        return playerAI != null;
    }
}
