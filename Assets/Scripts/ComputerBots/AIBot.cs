using System.Collections.Generic;

public interface AIBot
{
    public bool BotBuyEnterpriseOrNot(Player player, Enterprise enterprise);
    public bool BotPayToGoOutOfPrisonOrNot(Player player);
    public bool BotStayOnWorkOrNot(Player player);
    public int BotWhichEnterprisePawn(Player player, List<Enterprise> enterprises);
    public int BotWhichEnterpriseUnPawn(Player player, List<Enterprise> pawnedEnterprises, int playerMoneyLeft);
    public int BotWhichEnterpriseBuildHotel(Player player, List<Enterprise> enterprisesToBuildHotel, int playerMoneyLeft);
}
