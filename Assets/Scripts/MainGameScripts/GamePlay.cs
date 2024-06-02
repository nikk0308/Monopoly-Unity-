using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GamePlay
{   
    private readonly int indexOfEndOfCountry;
    private readonly int indexOfEndOfArray;
    private readonly int indexOfWorkCell;
    private readonly int enterOnArrayInAnother;
    
    private Field field;
    private List<Player> players;
    private List<bool> isPlayersAlive;
    private int playersLeftAmount;
    private int curPlayerIndex;
    public bool isNextMoveNeed;
    private Player curPlayer;

    private bool isUnfinishedMethod;
    private bool isStayedMethodPerformed;

    private Position prePosition;
    private Position postPosition;
    private string messageToPrint;

    private string messageToAdd;
    private string textFromBlock1;
    private string textFromBlock2;

    public Field CurField => field;
    public List<Player> Players => players;
    public static GamePlay Instance { get; private set; }  

    public GamePlay() {
        indexOfEndOfCountry = Field.specialIndexesByCellNames["ExitChance"];
        indexOfEndOfArray = Field.arrayLength - 1;
        indexOfWorkCell = Field.specialIndexesByCellNames["Work"];
        enterOnArrayInAnother = indexOfWorkCell;
    }

    public static GamePlay GetInstance() {
        if (Instance == null) {
            Instance = new GamePlay();
        }
        return Instance;
    }

    public void StartGameUpdate(List<Player> playersList) {
        RecreateField();
        Instance.players = playersList;
        playersLeftAmount = playersList.Count;
        curPlayerIndex = 0;
        curPlayer = playersList[curPlayerIndex];
        isPlayersAlive = new();
        for (int i = 0; i < playersLeftAmount; i++) {
            isPlayersAlive.Add(true);
        }

        for (int i = 0; i < playersLeftAmount; i++) {
            GameShowManager.Instance.MovePlayerToCurPos(i, playersList[i].positionInField,
                playersList[i].positionInField);
        }

        GameShowManager.Instance.UpdateCurrentTurnLabel(curPlayerIndex);
        GameShowManager.Instance.SetButtonText(Constants.StartGameButtonText);
        GameShowManager.Instance.LockDiceButton(false);
        GameShowManager.Instance.LockPreTurnButtons(true);
        GameShowManager.Instance.FieldToShow.ShowInfoBlock1(false);
        GameShowManager.Instance.FieldToShow.ShowInfoBlock2(false);
        GameShowManager.Instance.FieldToShow.ShowButtonsBlock2(false);
        textFromBlock1 = "";
        textFromBlock2 = "";
    }

    private void RecreateField() {
        Instance.field = new Field();
    }

    public void MakeTurnIfStayed() {
        isStayedMethodPerformed = true;
        GameShowManager.Instance.FieldToShow.ShowInfoBlock1(false);
        GameShowManager.Instance.FieldToShow.ShowInfoBlock2(false);
        GameShowManager.Instance.LockDiceButton(true);
        GameShowManager.Instance.LockPreTurnButtons(false);
        textFromBlock1 = "";
        textFromBlock2 = "";
        GameShowManager.Instance.UpdateCurrentTurnLabel(curPlayerIndex);
        if (curPlayer.IsABot()) {
            GameShowManager.Instance.LockPreTurnButtons(true);
            GameShowManager.Instance.UnPawnFromBank();
            GameShowManager.Instance.BuildHome();
        }
        
        players[curPlayerIndex].MakeTurnForPawnedEnter(field);
        prePosition = Position.CreatePrePosition(curPlayer.positionInField);
        field.TakeCardByPlayerPos(curPlayer.positionInField).DoActionIfStayed(field, curPlayer, out isNextMoveNeed, 
            out isUnfinishedMethod, ref textFromBlock1, ref textFromBlock2);
        postPosition = players[curPlayerIndex].positionInField;
        GameShowManager.Instance.MovePlayerToCurPos(curPlayerIndex, prePosition, postPosition);
        
        GameShowManager.Instance.FieldToShow.ShowInfoBlock1(true);
        GameShowManager.Instance.FieldToShow.ShowInfoBlock2(true);
        GameShowManager.Instance.FieldToShow.SetTextInfo1(textFromBlock1);
        GameShowManager.Instance.FieldToShow.SetTitleInfo2(Constants.GetCardNameByPos(prePosition));
        GameShowManager.Instance.FieldToShow.SetTextInfo2(textFromBlock2);

        GameShowManager.Instance.UpdateAllPlayersInfo(players, field);
        if (!isUnfinishedMethod) {
            MakeTurnIfStayedContinue(true);
        }
        else {
            GameShowManager.Instance.FieldToShow.ShowButtonsBlock2(true);
        }
    }

    public void MakeTurnIfStayedContinue(bool isYesTapped) {
        if (isUnfinishedMethod) {
            GameShowManager.Instance.FieldToShow.ShowButtonsBlock2(false);
            field.TakeCardByPlayerPos(curPlayer.positionInField).DoActionIfStayedAndUnfinished(field, 
                curPlayer, isYesTapped, out isNextMoveNeed, ref textFromBlock1, ref textFromBlock2);
            GameShowManager.Instance.FieldToShow.SetTextInfo1(textFromBlock1);
            GameShowManager.Instance.FieldToShow.SetTitleInfo2(Constants.GetCardNameByPos(prePosition));
            GameShowManager.Instance.FieldToShow.SetTextInfo2(textFromBlock2);
        }
        GameShowManager.Instance.UpdateAllPlayersInfo(players, field);
        
        if (isNextMoveNeed) {
            GameShowManager.Instance.SetButtonText(Constants.RollDiceButtonText);
        }
        else {
            //GameShowManager.Instance.LockPreTurnButtons(true);
            MoveToNextTurn();
        }

        GameShowManager.Instance.LockDiceButton(false);
    }
    public void MakeTurnAfterRollDice(int diceNum) {
        isStayedMethodPerformed = false;
        GameShowManager.Instance.FieldToShow.ShowInfoBlock1(false);
        GameShowManager.Instance.FieldToShow.ShowInfoBlock2(false);
        GameShowManager.Instance.LockDiceButton(true);
        GameShowManager.Instance.LockPreTurnButtons(true);
        
        prePosition = Position.CreatePrePosition(curPlayer.positionInField);
        TurnWithDice(curPlayer, diceNum);
        
        field.TakeCardByPlayerPos(curPlayer.positionInField).DoActionIfArrived(field, curPlayer, out isUnfinishedMethod, 
            ref textFromBlock1, ref textFromBlock2);
        postPosition = curPlayer.positionInField;
        GameShowManager.Instance.MovePlayerToCurPos(curPlayerIndex, prePosition, postPosition);
        
        GameShowManager.Instance.FieldToShow.ShowInfoBlock1(true);
        GameShowManager.Instance.FieldToShow.ShowInfoBlock2(true);
        //messageToAdd = "Гравець " + players[curPlayerIndex].nameInGame + " прибув на карту " + Constants.FullSellNameByPos(postPosition, field);
        //GameShowManager.Instance.FieldToShow.SetAdditionTextInfo1(messageToAdd);
        GameShowManager.Instance.FieldToShow.SetTextInfo1(textFromBlock1);
        GameShowManager.Instance.FieldToShow.SetTitleInfo2(Constants.GetCardNameByPos(postPosition));
        GameShowManager.Instance.FieldToShow.SetTextInfo2(textFromBlock2);
        
        GameShowManager.Instance.UpdateAllPlayersInfo(players, field);
        if (!isUnfinishedMethod) {
            MakeTurnAfterRollDiceContinue(true);
        }
        else {
            GameShowManager.Instance.FieldToShow.ShowButtonsBlock2(true);
        }
    }

    public void MakeTurnAfterRollDiceContinue(bool isYesTapped) {
        if (isUnfinishedMethod) {
            GameShowManager.Instance.FieldToShow.ShowButtonsBlock2(false);
            field.TakeCardByPlayerPos(curPlayer.positionInField).DoActionIfArrivedAndUnfinished(field, 
                players[curPlayerIndex], isYesTapped, ref textFromBlock1, ref textFromBlock2);
            GameShowManager.Instance.FieldToShow.SetTextInfo1(textFromBlock1);
            GameShowManager.Instance.FieldToShow.SetTitleInfo2(Constants.GetCardNameByPos(postPosition));
            GameShowManager.Instance.FieldToShow.SetTextInfo2(textFromBlock2);
        }
        GameShowManager.Instance.UpdateAllPlayersInfo(players, field);

         if (IsPossibleBankrupt(curPlayer)) {
             GameShowManager.Instance.SetButtonText(Constants.BankruptButtonText);
             GameShowManager.Instance.LockDiceButton(false);
             GameShowManager.Instance.LockPreTurnButtons(false);
             if (!curPlayer.IsABot()) {
                 PlayerInfoManager.Instance.ShowError("Баланс " + curPlayer.nameInGame + " опустився нижче нуля. Необхідно " +
                                                      "погасити борг, заклавши підприємства, для продовження гри");
             }
             GameShowManager.Instance.PawnInBank();
         }
         else {
             MoveToNextTurn();
         }
    }

    public void MoveToNextTurn() {
        curPlayerIndex = NextPlayerIndex();
        curPlayer = players[curPlayerIndex];
        GameShowManager.Instance.SetButtonText(Constants.NextTurnButtonText);
        GameShowManager.Instance.LockDiceButton(false);
    }

    public void CurPlayerIsBankrupt() {
        curPlayer.FreeAllEnterprises(field);
        GameShowManager.Instance.RemovePlayerFromField(curPlayerIndex, curPlayer.positionInField);
        //GameShowManager.Instance.UpdatePlayerInfo(curPlayer, field, curPlayerIndex);
        GameShowManager.Instance.PlayerBankruptUpdate(curPlayerIndex);
        isPlayersAlive[curPlayerIndex] = false;
        playersLeftAmount--;
        
        if (playersLeftAmount == 1) {
            curPlayerIndex = NextPlayerIndex();
            curPlayer = players[curPlayerIndex];
            GameShowManager.Instance.RemovePlayerFromField(curPlayerIndex, curPlayer.positionInField);
            GameShowManager.Instance.GameEnded(curPlayer);
        }
        else {
            PlayerInfoManager.Instance.ShowError("Гравець " + curPlayer.nameInGame + " тепер банкрот");
            MoveToNextTurn();
        }
    }

    public void ContinueAfterSelectedAns(bool isYes) {
        if (isStayedMethodPerformed) {
            MakeTurnIfStayedContinue(isYes);
        }
        else {
            MakeTurnAfterRollDiceContinue(isYes);
        }
    }

    private void TurnWithDice(Player player, int diceNum) {
        int curPlayerArr = player.positionInField.arrayIndex;
        int curPlayerCell = player.positionInField.cellIndex;
        int newPlayerCell = curPlayerCell + diceNum;

        if (curPlayerCell < indexOfWorkCell && newPlayerCell >= indexOfWorkCell) {
            string strToShowInfo = OutputPhrases.TextGainSalary(player, Constants.Salary);
            GameShowManager.Instance.FieldToShow.AutoAddText(ref textFromBlock1, ref textFromBlock2, strToShowInfo);
            player.moneyAmount += Constants.Salary;
        }

        if (curPlayerCell == indexOfEndOfCountry) {
            player.positionInField.cellIndex = newPlayerCell;
        }
        else if (curPlayerCell < indexOfEndOfCountry) {
            player.positionInField.cellIndex = newPlayerCell % (indexOfEndOfCountry + 1);
        }
        else {
            // curPlayerCell > indexOfEndOfCountry
            if (newPlayerCell <= indexOfEndOfArray) {
                player.positionInField.cellIndex = newPlayerCell;
            }
            else {
                player.positionInField.arrayIndex = curPlayerArr == 0 ? 1 : 0;
                player.positionInField.cellIndex = newPlayerCell - indexOfEndOfArray - 1 + enterOnArrayInAnother;
            }
        }
    }

    private int NextPlayerIndex() {
        int i = curPlayerIndex;
        bool isPlaying = false;
        while (!isPlaying) {
            i = (i + 1) % players.Count;
            isPlaying = isPlayersAlive[i];
        }
        return i;
    }

    private bool IsPossibleBankrupt(Player player) {
        return player.moneyAmount < 0;
    }

    public Player CurrentPlayer() {
        return curPlayer;
    }

    /*public void StartGame(Player[] players) {
        bool isGameEnd = false;
        List<Player> playersInGame = players.ToList();
        int curIndexPlayerTurn = 0;
        Player curPlayer;
        bool isNextMoveNeed;
        string messageToPrint;

        while (!isGameEnd) {
            JustOutput.PrintAllField(field, playersInGame);
            curPlayer = playersInGame[curIndexPlayerTurn];
            PreTurnThings(curPlayer, playersInGame);

            //messageToPrint = field.TakeCardByPlayerPos(curPlayer.positionInField).DoActionIfStayed(field, curPlayer, out isNextMoveNeed, out isUnfinishedMethod);
            //JustOutput.PrintText(messageToPrint);

            //if (isNextMoveNeed) {
                //PlayerTurnWithDice(curPlayer);
                JustOutput.PrintText(OutputPhrases.PlayerMovedTo(curPlayer, field));
                //messageToPrint = field.TakeCardByPlayerPos(curPlayer.positionInField).DoActionIfArrived(field, curPlayer, out isUnfinishedMethod);
                //JustOutput.PrintText(messageToPrint);
            //}

            if (IsPlayerGoOut(curPlayer)) {
                playersInGame.RemoveAt(curIndexPlayerTurn);
                curIndexPlayerTurn--;
                if (playersInGame.Count == 1) {
                    isGameEnd = true;
                    JustOutput.Congratulations(playersInGame[0], field);
                }
            }

            curIndexPlayerTurn = (curIndexPlayerTurn + 1) % playersInGame.Count;

            JustOutput.PrintText(OutputPhrases.TextPressEnterToGoNextPlayer());
            //Interactive.PressEnter();
        }
    }

    private bool IsPlayerGoOut(Player player) {
        if (player.moneyAmount >= 0) {
            return false;
        }

        JustOutput.PrintText(OutputPhrases.TextYouMustPawnEnterprises());

        List<Enterprise> enterprises = player.GetPawnedOrNotPlayerEnterprises(field, false);
        while (enterprises.Count > 0 && player.moneyAmount < 0) {
            JustOutput.PrintDebtAndUnPawnEnterprises(player, enterprises);

            JustOutput.PrintText(OutputPhrases.TextInputEnterpriseNum(enterprises.Count));
            int enterpriseToPawn = player.WhichEnterprisePawnToNotLose(enterprises);

            enterprises[enterpriseToPawn].PawnInBank(field);
            enterprises.RemoveAt(enterpriseToPawn);

            if (player.moneyAmount < 0) {
                JustOutput.PrintText(OutputPhrases.TextPawnToNotLost(player, "noEnough"));
            }
        }

        if (player.moneyAmount >= 0) {
            JustOutput.PrintText(OutputPhrases.TextPawnToNotLost(player, "backInGame"));
            return false;
        }

        JustOutput.PrintText(OutputPhrases.TextPawnToNotLost(player, "lost"));
        player.FreeAllEnterprises(field); // Tut.
        return true;
    }

    private void PreTurnThings(Player player, List<Player> playersInGame) {
        player.MakeTurnForPawnedEnter(field);
        JustOutput.PrintPlayersInfo(playersInGame, field);
        PawnEnterpriseOrBuildHotel(player);
    }

    private void PawnEnterpriseOrBuildHotel(Player player) {
        List<Enterprise> notPawnedEnterprises = player.GetPawnedOrNotPlayerEnterprises(field, false);
        List<Enterprise> pawnedEnterprises = player.GetPawnedOrNotPlayerEnterprises(field, true);
        List<Enterprise> enterprisesToBuildHotel = player.GetFullIndustryWithoutNHotelsEnterprises(field);
        bool isContinue = true;

        JustOutput.PrintText(OutputPhrases.TextPreTurnMainOutput(player, notPawnedEnterprises.Count,
            pawnedEnterprises.Count, enterprisesToBuildHotel.Count));

        do {
            JustOutput.PrintText(OutputPhrases.TextGetNumOfPreTurnAction());
            string actionNum =
                player.PawnEnterpriseOrBuildHotelPreTurn(notPawnedEnterprises, pawnedEnterprises,
                    enterprisesToBuildHotel);
            switch (actionNum) {
                case "1":
                    if (notPawnedEnterprises.Count == 0) {
                        JustOutput.PrintText(OutputPhrases.TextNoEnterprisesFor("pawn"));
                    }
                    else {
                        JustOutput.PrintEnterprises(notPawnedEnterprises, "notPawned");
                        JustOutput.PrintText(OutputPhrases.TextInputEnterpriseNum(notPawnedEnterprises.Count));
                        int enterpriseNum = player.WhichEnterprisePawnPreTurn(notPawnedEnterprises);

                        notPawnedEnterprises[enterpriseNum].PawnInBank(field);
                        notPawnedEnterprises.RemoveAt(enterpriseNum);
                    }

                    break;
                case "2":
                    if (pawnedEnterprises.Count == 0) {
                        JustOutput.PrintText(OutputPhrases.TextNoEnterprisesFor("unPawn"));
                    }
                    else {
                        JustOutput.PrintEnterprises(pawnedEnterprises, "pawned");
                        JustOutput.PrintText(OutputPhrases.TextInputEnterpriseNum(pawnedEnterprises.Count));
                        int enterpriseNum = player.WhichEnterpriseUnPawnPreTurn(pawnedEnterprises);

                        if (player.moneyAmount > pawnedEnterprises[enterpriseNum].priceToBuy) {
                            pawnedEnterprises[enterpriseNum].UnPawnFromBank(field);
                            pawnedEnterprises.RemoveAt(enterpriseNum);
                        }
                        else {
                            JustOutput.PrintText(OutputPhrases.TextNoMoneyForUnpawnOrBuild(true));
                        }
                    }

                    break;
                case "3":
                    if (enterprisesToBuildHotel.Count == 0) {
                        JustOutput.PrintText(OutputPhrases.TextNoEnterprisesFor("hotel"));
                    }
                    else {
                        JustOutput.PrintEnterprises(enterprisesToBuildHotel, "hotel");
                        JustOutput.PrintText(OutputPhrases.TextInputEnterpriseNum(enterprisesToBuildHotel.Count));
                        int enterpriseNum = player.WhichEnterpriseBuildHotelPreTurn(enterprisesToBuildHotel);

                        if (player.moneyAmount > enterprisesToBuildHotel[enterpriseNum].priceToBuildHotel) {
                            enterprisesToBuildHotel[enterpriseNum].BuildHomeInEnterprise();
                            enterprisesToBuildHotel.RemoveAt(enterpriseNum);
                        }
                        else {
                            JustOutput.PrintText(OutputPhrases.TextNoMoneyForUnpawnOrBuild(false));
                        }
                    }

                    break;
                default:
                    Console.WriteLine();
                    isContinue = false;
                    break;
            }
        } while (isContinue);
    }
    */
}
