using System.Collections.Generic;
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
                 MessageWindow.Instance.ShowMessage("Баланс " + curPlayer.nameInGame + " опустився нижче нуля. Необхідно " +
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
            MessageWindow.Instance.ShowMessage("Гравець " + curPlayer.nameInGame + " тепер банкрот");
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
}
