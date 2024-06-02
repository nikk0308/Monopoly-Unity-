using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysAgreeIfCanBot : AIBot
{
    public bool BotBuyEnterpriseOrNot(Player player, Enterprise enterprise) {
        return true;
    }

    public bool BotPayToGoOutOfPrisonOrNot(Player player) {
        return true;
    }

    public bool BotStayOnWorkOrNot(Player player) {
        return true;
    }

    public int BotWhichEnterprisePawn(Player player, List<Enterprise> notPawnedEnterprises) {
        if (notPawnedEnterprises.Count == 0) {
            return -1;
        }
        return 0;
    }
    
    public int BotWhichEnterpriseUnPawn(Player player, List<Enterprise> pawnedEnterprises, int playerMoneyLeft) {
        if (pawnedEnterprises.Count == 0 || pawnedEnterprises[0].priceToBuy > playerMoneyLeft) {
            return -1;
        }
        return 0;
    }

    public int BotWhichEnterpriseBuildHotel(Player player, List<Enterprise> enterprisesToBuildHotel, int playerMoneyLeft) {
        if (enterprisesToBuildHotel.Count == 0 || enterprisesToBuildHotel[0].priceToBuildHotel > playerMoneyLeft) {
            return -1;
        }
        return 0;
    }
}
