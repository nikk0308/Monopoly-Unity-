using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Player
{
    internal readonly string nameInGame;
    internal readonly Color chipColor;
    internal int moneyAmount;
    internal Position? positionInField;
    internal int turnsToGoOutOfPrison;
    internal int howManyTimesPayedInPrison;
    internal bool canGoOutOfCountry;
    internal int turnsCanContinueWork;
    internal int howManyTimesWorked;
    private AIBot? playerAI;

    public Player(string nameInGame, AIBot? playerAI, Color chipColor, int moneyAmount = 0,
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


    public bool BuyEnterpriseOrNotBot(Enterprise enterprise) {
        return playerAI.BotBuyEnterpriseOrNot(this, enterprise);
    }

    public bool PayToGoOutOfPrisonOrNotBot() {
        return playerAI.BotPayToGoOutOfPrisonOrNot(this);
    }

    public bool StayOnWorkOrNotBot() {
        return playerAI.BotStayOnWorkOrNot(this);
    }

    public int WhichEnterprisePawnBot(List<Enterprise> enterprises) {
        return playerAI.BotWhichEnterprisePawn(this, enterprises);
    }

    public int WhichEnterpriseUnPawnBot(List<Enterprise> pawnedEnterprises, int playerMoneyLeft) {
        return playerAI.BotWhichEnterpriseUnPawn(this, pawnedEnterprises, playerMoneyLeft);
    }

    public int WhichEnterpriseBuildHotelBot(List<Enterprise> enterprisesToBuildHotel, int playerMoneyLeft) {
        return playerAI.BotWhichEnterpriseBuildHotel(this, enterprisesToBuildHotel, playerMoneyLeft);
    }

    public bool IsABot() {
        return playerAI != null;
    }
}
