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

    private string[] textToShow;

    public Enterprise(int priceToBuy, Industry industry, string title, Player? owner = null,
        int turnsToDisappearIfPawned = 0, bool isBuiltHotel = false, bool isFullIndustry = false) {
        this.owner = owner;
        this.industry = industry;
        this.title = title;

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

    public override string[] TextToPrintInAField {
        get { return textToShow; }
    }

    public override string DoActionIfArrived(Field field, Player player) {
        return PayBuyOrStay(field, player);
    }

    public override string DoActionIfStayed(Field field, Player player, out bool isNextMoveNeed) {
        return JustTurn(player, out isNextMoveNeed);
    }

    public void PawnInBank(Field field) {
        owner.moneyAmount += currentPriceOthersPay;
        JustOutput.PrintText(OutputPhrases.TextPawnInBank(this));
        CollectOrDestroyIndustry(field, owner, false);
        currentPriceOthersPay = priceOthersPayLevel1;
        turnsToDisappearIfPawned = turnsInGeneralToDisappIfPawned;
        UpdateTextToShow();
    }

    public void UnPawnFromBank(Field field) {
        owner.moneyAmount -= priceToBuy;
        JustOutput.PrintText(OutputPhrases.TextUnPawnFromBank(this));
        turnsToDisappearIfPawned = 0;
        CollectOrDestroyIndustry(field, owner, true);
        UpdateTextToShow();
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
        UpdateTextToShow();
    }

    public void ReduceTurnsAmount() {
        turnsToDisappearIfPawned--;
        UpdateTextToShow();
    }

    public void BuildHomeInEnterprise() {
        isBuiltHotel = true;
        currentPriceOthersPay = priceOthersPayLevel3;
        owner.moneyAmount -= priceToBuildHotel;
        JustOutput.PrintText(OutputPhrases.TextBuildHome(this));
        UpdateTextToShow();
    }

    private string PayBuyOrStay(Field field, Player player) {
        if (this.owner == player) {
            return OutputPhrases.TextPayBuyOrStay(player, this, "inHome");
        }

        if (this.owner != null) {
            if (IsPawned()) {
                return OutputPhrases.TextPayBuyOrStay(player, this, "inBank");
            }
            else if (owner.IsInPrison()) {
                return OutputPhrases.TextPayBuyOrStay(player, this, "inPrison");
            }
            else {
                player.moneyAmount -= currentPriceOthersPay;
                this.owner.moneyAmount += currentPriceOthersPay;
                return OutputPhrases.TextPayBuyOrStay(player, this, "payAnotherPerson");
            }
        }

        if (player.moneyAmount < priceToBuy) {
            return OutputPhrases.TextPayBuyOrStay(player, this, "noMoneyToBuy");
        }

        JustOutput.PrintText(OutputPhrases.TextBuyEnterpriseOrNot(player, this));
        string playerChoice = player.BuyEnterpriseOrNot(this);

        if (playerChoice == "1") {
            BuyingCard(field, player);
            return OutputPhrases.TextPayBuyOrStay(player, this, "bought");
        }

        return OutputPhrases.TextPayBuyOrStay(player, this, "discard");
    }

    private void UpdateTextToShow() {
        textToShow = OutputPhrases.TextToShowEnterprise(this);
    }

    private void BuyingCard(Field field, Player player) {
        player.moneyAmount -= priceToBuy;
        owner = player;
        currentPriceOthersPay = priceOthersPayLevel1;
        CollectOrDestroyIndustry(field, player, true);
        UpdateTextToShow();
    }

    private void UpdateIfFullIndustry() {
        currentPriceOthersPay = priceOthersPayLevel2;
        isFullIndustry = true;
        UpdateTextToShow();
    }

    private void UpdateIfDestroyedIndustry() {
        currentPriceOthersPay = priceOthersPayLevel1;
        isBuiltHotel = false;
        isFullIndustry = false;
        UpdateTextToShow();
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
}
