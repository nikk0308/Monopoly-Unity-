using System.Collections.Generic;

public class ZhlobBot : AIBot
{
    private const int upperPercentFromBalanceToBuy = 50;
    private const int upperPercentFromBalanceToUnpawn = 80;
    private const int upperPercentFromBalanceToBuildHotel = 60;

    public bool BotBuyEnterpriseOrNot(Player player, Enterprise enterprise) {
        return IsNormalPriceFor(player.moneyAmount, enterprise.priceToBuy, "buy");
    }

    public bool BotPayToGoOutOfPrisonOrNot(Player player) {
        return false;
    }

    public bool BotStayOnWorkOrNot(Player player) {
        return true;
    }

    public int BotWhichEnterprisePawn(Player player, List<Enterprise> notPawnedEnterprises) {
        return FindEnterpriseMinPrice(notPawnedEnterprises, player.moneyAmount, "pawn");
    }

    public int BotWhichEnterpriseUnPawn(Player player, List<Enterprise> pawnedEnterprises, int playerMoneyLeft) {
        return FindEnterpriseMinPrice(pawnedEnterprises, playerMoneyLeft, "unpawn");
    }

    public int BotWhichEnterpriseBuildHotel(Player player, List<Enterprise> enterprisesToBuildHotel, int playerMoneyLeft) {
        return FindEnterpriseMinPrice(enterprisesToBuildHotel, playerMoneyLeft, "build");
    }

    private int FindEnterpriseMinPrice(List<Enterprise> enterprises, int playerMoneyLeft, string purpose) {
        if (enterprises.Count == 0) {
            return -1;
        }
        
        int index = 0;
        int price = EnterprisePrice(enterprises[index], purpose);
        for (int i = 1; i < enterprises.Count; i++) {
            if (EnterprisePrice(enterprises[index], purpose) < price) {
                index = i;
                price = EnterprisePrice(enterprises[index], purpose);
            }
        }

        if (!IsNormalPriceFor(playerMoneyLeft, price, purpose)) {
            return -1;
        }
        return index;
    }

    private int EnterprisePrice(Enterprise enterprise, string purpose) {
        return purpose switch {
            "pawn" => enterprise.currentPriceOthersPay,
            "unpawn" => enterprise.priceToBuy,
            "build" => enterprise.priceToBuildHotel,
        };
    }

    private bool IsNormalPriceFor(int moneyLeft, int price, string purpose) {
        return purpose switch {
            "pawn" => true,
            "unpawn" => moneyLeft * upperPercentFromBalanceToUnpawn / 100 >= price,
            "build" => moneyLeft * upperPercentFromBalanceToBuildHotel / 100 >= price,
            "buy" => moneyLeft * upperPercentFromBalanceToBuy / 100 >= price,
        };
    }
}
