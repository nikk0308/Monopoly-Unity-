using UnityEngine;

public class Enterprise : Card
{
    public Player? owner;
    public Industry industry;
    public string title;

    public readonly int priceToBuy;
    public readonly int priceToBuildHotel;

    public int turnsToDisappearIfPawned;
    private const int turnsInGeneralToDisappIfPawned = 5;

    public bool isBuiltHotel;
    public bool isFullIndustry;
    public readonly int priceOthersPayLevel1;
    public readonly int priceOthersPayLevel2;
    public readonly int priceOthersPayLevel3;
    public int currentPriceOthersPay;

    private Position position;
    
    private string[] textToShow;

    public Enterprise(int priceToBuy, Industry industry, string title, Position position, Player? owner = null,
        int turnsToDisappearIfPawned = 0, bool isBuiltHotel = false, bool isFullIndustry = false) {
        this.owner = owner;
        this.industry = industry;
        this.title = title;
        this.position = position;

        this.priceToBuy = priceToBuy;
        priceToBuildHotel = priceToBuy * 3;

        this.turnsToDisappearIfPawned = turnsToDisappearIfPawned;

        this.isBuiltHotel = isBuiltHotel;
        this.isFullIndustry = isFullIndustry;
        priceOthersPayLevel1 = priceToBuy / 2;
        priceOthersPayLevel2 = priceToBuy;
        priceOthersPayLevel3 = priceToBuy * 2;
        currentPriceOthersPay = 0;

        UpdateTextToShow();
    }

    public override void DoActionIfArrived(Field field, Player player, out bool isUnfinishedMethod, ref string text1, ref string text2) {
        PayBuyOrStay(field, player, out isUnfinishedMethod, ref text1, ref text2);
    }

    public override void DoActionIfStayed(Field field, Player player, out bool isNextMoveNeed, out bool isUnfinishedMethod, 
        ref string text1, ref string text2) {
        JustTurn(player, out isNextMoveNeed, out isUnfinishedMethod, ref text1, ref text2);
    }
    public override void DoActionIfArrivedAndUnfinished(Field field, Player player, bool yesOrNo, ref string text1, ref string text2) {
        PayBuyOrStayContinue(field, player, yesOrNo, ref text1, ref text2);
    }

    public override void DoActionIfStayedAndUnfinished(Field field, Player player, bool yesOrNo, out bool isNextMoveNeed, 
        ref string text1, ref string text2) {
        isNextMoveNeed = false;
    }

    public void PawnInBank(Field field) {
        //owner.moneyAmount += currentPriceOthersPay;
        JustOutput.PrintText(OutputPhrases.TextPawnInBank(this));
        CollectOrDestroyIndustry(field, owner, false);
        currentPriceOthersPay = priceOthersPayLevel1;
        turnsToDisappearIfPawned = turnsInGeneralToDisappIfPawned;
        GetGraphicEnterprise().PawnEnterprise(true, turnsToDisappearIfPawned);
    }

    public void UnPawnFromBank(Field field) {
        owner.moneyAmount -= priceToBuy;
        JustOutput.PrintText(OutputPhrases.TextUnPawnFromBank(this));
        turnsToDisappearIfPawned = 0;
        CollectOrDestroyIndustry(field, owner, true);
        GetGraphicEnterprise().PawnEnterprise(false);
    }

    public bool IsPawned() {
        return turnsToDisappearIfPawned > 0;
    }

    public void ClearEnterprise() {
        // Tut?
        owner = null;
        turnsToDisappearIfPawned = 0;
        isBuiltHotel = false;
        isFullIndustry = false;
        currentPriceOthersPay = 0;
        GetGraphicEnterprise().SetOwnerColorToNone();
        GetGraphicEnterprise().SetCurrentActivePriceToNone();
        GetGraphicEnterprise().SetPricesColorsToDefault();
        GetGraphicEnterprise().PawnEnterprise(false);
    }

    public void ReduceTurnsAmount() {
        turnsToDisappearIfPawned--;
        GetGraphicEnterprise().SetTurnsToDisappear(turnsToDisappearIfPawned);
    }

    public void BuildHomeInEnterprise() {
        isBuiltHotel = true;
        currentPriceOthersPay = priceOthersPayLevel3;
        owner.moneyAmount -= priceToBuildHotel;
        GetGraphicEnterprise().SetCurrentActivePrice(3);
        JustOutput.PrintText(OutputPhrases.TextBuildHome(this));
    }

    private void PayBuyOrStay(Field field, Player player, out bool isUnfinishedMethod, ref string text1, ref string text2) {
        isUnfinishedMethod = false;
        string curStrShow;
        if (owner == player) {
            curStrShow = OutputPhrases.TextPayBuyOrStay(player, this, "inHome");
            GameShowManager.Instance.FieldToShow.AutoAddText(ref text1, ref text2, curStrShow);
            return;
        }

        if (owner != null) {
            if (IsPawned()) {
                curStrShow = OutputPhrases.TextPayBuyOrStay(player, this, "inBank");
                GameShowManager.Instance.FieldToShow.AutoAddText(ref text1, ref text2, curStrShow);
                return;
            }
            if (owner.IsInPrison()) {
                curStrShow = OutputPhrases.TextPayBuyOrStay(player, this, "inPrison");
                GameShowManager.Instance.FieldToShow.AutoAddText(ref text1, ref text2, curStrShow);
                return;
            }
            player.moneyAmount -= currentPriceOthersPay;
            owner.moneyAmount += currentPriceOthersPay;
            curStrShow = OutputPhrases.TextPayBuyOrStay(player, this, "payAnotherPerson");
            GameShowManager.Instance.FieldToShow.AutoAddText(ref text1, ref text2, curStrShow);
            return;
        }

        if (player.moneyAmount < priceToBuy) {
            curStrShow = OutputPhrases.TextPayBuyOrStay(player, this, "noMoneyToBuy");
            GameShowManager.Instance.FieldToShow.AutoAddText(ref text1, ref text2, curStrShow);
            return;
        }

        if (player.IsABot()) {
            bool botChoice = player.BuyEnterpriseOrNotBot(this);
            PayBuyOrStayContinue(field, player, botChoice, ref text1, ref text2);
            return;
        }
        
        isUnfinishedMethod = true;
        curStrShow = OutputPhrases.TextBuyEnterpriseOrNot(player, this);
        GameShowManager.Instance.FieldToShow.AutoAddText(ref text1, ref text2, curStrShow);

        // GameShowManager.Instance.FieldToShow.SetTextInfo2(OutputPhrases.TextBuyEnterpriseOrNot(player, this));
        // //JustOutput.PrintText(OutputPhrases.TextBuyEnterpriseOrNot(player, this));
        // string playerChoice = player.BuyEnterpriseOrNot(this).Result;
        //
        // Debug.Log("End curwa");
        //
        // if (playerChoice == "1") {
        //     BuyingCard(field, player);
        //     return OutputPhrases.TextPayBuyOrStay(player, this, "bought");
        // }
        //
        // return OutputPhrases.TextPayBuyOrStay(player, this, "discard");
    }

    private void PayBuyOrStayContinue(Field field, Player player, bool yesOrNo, ref string text1, ref string text2) {
        string curStrShow;
        if (yesOrNo) {
            BuyingCard(field, player);
            curStrShow = OutputPhrases.TextPayBuyOrStay(player, this, "bought");
            GameShowManager.Instance.FieldToShow.AutoAddText(ref text1, ref text2, curStrShow);
            return;
        }
        curStrShow = OutputPhrases.TextPayBuyOrStay(player, this, "discard");
        GameShowManager.Instance.FieldToShow.AutoAddText(ref text1, ref text2, curStrShow);
    }

    private void UpdateTextToShow() {
        textToShow = OutputPhrases.TextToShowEnterprise(this);
    }

    private void BuyingCard(Field field, Player player) {
        player.moneyAmount -= priceToBuy;
        owner = player;
        currentPriceOthersPay = priceOthersPayLevel1;
        GetGraphicEnterprise().SetOwnerColor(player.chipColor);
        GetGraphicEnterprise().SetCurrentActivePrice(1);
        CollectOrDestroyIndustry(field, player, true);
    }

    private void UpdateIfFullIndustry() {
        currentPriceOthersPay = priceOthersPayLevel2;
        GetGraphicEnterprise().SetCurrentActivePrice(2);
        isFullIndustry = true;
    }

    private void UpdateIfDestroyedIndustry() {
        currentPriceOthersPay = priceOthersPayLevel1;
        GetGraphicEnterprise().SetCurrentActivePrice(1);
        isBuiltHotel = false;
        isFullIndustry = false;
    }

    private void CollectOrDestroyIndustry(Field field, Player player, bool isToCollect) {
        bool isFullIndustryCur = true;
        foreach (var enterprise in industry.GetEnterprisesInIndustry(field)) {
            if (enterprise.owner != player || enterprise.IsPawned()) {
                isFullIndustryCur = false;
            }
        }

        if (isFullIndustryCur) {
            JustOutput.PrintText(OutputPhrases.TextGainOrLostLocalMonopoly(player, this, isToCollect));

            foreach (var enterprise in industry.GetEnterprisesInIndustry(field)) {
                if (isToCollect) {
                    enterprise.UpdateIfFullIndustry();
                }
                else {
                    enterprise.UpdateIfDestroyedIndustry();
                }
            }
        }
    }

    private EnterpriseInfo GetGraphicEnterprise() {
        return GameShowManager.Instance.FieldToShow.GetCardShowByPosition(position).info;
    }
}
