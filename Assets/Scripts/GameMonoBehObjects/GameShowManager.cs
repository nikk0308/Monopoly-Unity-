using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameShowManager : MonoBehaviour {
    
    [SerializeField] private TMP_Text curTurn;
    [SerializeField] private Button rollDice;
    [SerializeField] private CubeRollScript dice;
    
    [SerializeField] private Button pawnInBank;
    [SerializeField] private Button unpawnFromBank;
    [SerializeField] private Button buildHome;

    [SerializeField] private GameObject preTurnButtonsLock;
    [SerializeField] private GameObject diceButtonLock;
    
    [SerializeField] private Transform playersList;
    [SerializeField] private Transform preGamingPlayersInfo;

    [SerializeField] private FieldShow fieldToShow;

    [SerializeField] private PlayerShow playerSample;
    
    [SerializeField] private GameObject enterprisesList;

    private List<PlayerShow> _playersList = new();
    private GamePlay _gameLogic;
    public FieldShow FieldToShow => fieldToShow;
    public static GameShowManager Instance { get; private set; }     
    private void Awake() 
    {
        if (Instance != null && Instance != this) {
            Destroy(this);
        }
        else {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        _gameLogic = GamePlay.GetInstance();
    }
    
    private void Start() {
        rollDice.onClick.AddListener(OnClickPopularButton);
        pawnInBank.onClick.AddListener(PawnInBank);
        unpawnFromBank.onClick.AddListener(UnPawnFromBank);
        buildHome.onClick.AddListener(BuildHome);
    }

    public void EnableWindow() {
        gameObject.SetActive(true);
        _gameLogic.StartGameUpdate(PlayersListFill());
    }

    private List<Player> PlayersListFill() {
        _playersList.Clear();
        List<Player> list = new();
        foreach (Transform obj in playersList) {
            Destroy(obj.gameObject);
        }
        foreach (Transform info in preGamingPlayersInfo) {
            PlayerInfo curInfo = info.GetComponent<PlayerInfo>();
            Player newPlayer = new Player(curInfo.NamePlayer.text, Constants.GetPlayerBot(curInfo.BotType),
                curInfo.ColorChip.color);
            
            var curPlayer = Instantiate(playerSample, playersList);
            curPlayer.SetStartInfo(curInfo.ColorChip.color, curInfo.NamePlayer.text, newPlayer);
            _playersList.Add(curPlayer);
            list.Add(newPlayer);
        }
        return list;
    }

    public void UpdateCurrentTurnLabel(int playerIndex) {
        curTurn.text = Constants.CurTurnBegin + _playersList[playerIndex].NamePlayer;
    }

    public void MovePlayerToCurPos(int playerIndex, Position pastPos, Position curPos) {
        CardShow pastCard = fieldToShow.GetCardShowByPosition(pastPos);
        CardShow curCard = fieldToShow.GetCardShowByPosition(curPos);
        pastCard.MovePlayerFromCard(_playersList[playerIndex].ColorPlayer);
        curCard.MovePlayerOnCard(_playersList[playerIndex].ColorPlayer);
    }

    public void RemovePlayerFromField(int playerIndex, Position pastPos) {
        CardShow pastCard = fieldToShow.GetCardShowByPosition(pastPos);
        pastCard.MovePlayerFromCard(_playersList[playerIndex].ColorPlayer);
    }

    public void SetButtonText(string textToChange) {
        rollDice.GetComponentInChildren<TMP_Text>().text = textToChange;
    }

    public void LockPreTurnButtons(bool isLock) {
        preTurnButtonsLock.SetActive(isLock);
    }
    
    public void LockDiceButton(bool isLock) {
        diceButtonLock.SetActive(isLock);
    }

    private void OnClickPopularButton() {
        switch (rollDice.GetComponentInChildren<TMP_Text>().text) {
            case Constants.RollDiceButtonText:
                RollDice();
                break;
            case Constants.BankruptButtonText:
                IAmBankrupt();
                break;
            case Constants.StartGameButtonText:
            case Constants.NextTurnButtonText:
                NextTurn();
                break;
        }
    }

    public void UpdatePlayerInfo(Player player, Field field, int index) {
        _playersList[index].UpdateInfo(player.moneyAmount, player.GetPawnedOrNotPlayerEnterprises(field, false).Count,
            player.GetPawnedOrNotPlayerEnterprises(field, true).Count);
    }

    public void PlayerBankruptUpdate(int index) {
        _playersList[index].PlayerIsBankrupt();
    }

    public void UpdateAllPlayersInfo(List<Player> players, Field field) {
        for (int i = 0; i < players.Count; i++) {
            UpdatePlayerInfo(players[i], field, i);
        }
    }
    
    private void RollDice() {
        //field.Tresh();
        int num = Constants.RollDice();
        dice.RollCube(num);
        _gameLogic.MakeTurnAfterRollDice(num);
    }

    private void NextTurn() {
        _gameLogic.MakeTurnIfStayed();
    }

    private void IAmBankrupt() {
        _gameLogic.CurPlayerIsBankrupt();
    }

    public void PawnInBank() {
        PlayerEnterprisesShow.Instance.FillWindow(GamePlay.Instance.CurrentPlayer(), "pawn");
    }

    public void UnPawnFromBank() {
        PlayerEnterprisesShow.Instance.FillWindow(GamePlay.Instance.CurrentPlayer(), "unpawn");
    }

    public void BuildHome() {
        PlayerEnterprisesShow.Instance.FillWindow(GamePlay.Instance.CurrentPlayer(), "build");
    }

    public void PossibleReturnToGame() {
        Player curPlayer = GamePlay.Instance.CurrentPlayer();
        if (rollDice.GetComponentInChildren<TMP_Text>().text != Constants.BankruptButtonText || curPlayer.moneyAmount < 0) {
            if (curPlayer.IsABot()) {
                rollDice.onClick.Invoke();
            }
            return;
        }

        if (rollDice.GetComponentInChildren<TMP_Text>().text == Constants.BankruptButtonText) {
            PlayerInfoManager.Instance.ShowError("Гравець " + curPlayer.nameInGame + " погасив борг та повертається до гри!");
        }
        SetButtonText(Constants.NextTurnButtonText);
        GamePlay.Instance.MoveToNextTurn();
    }

    public void GameEnded(Player winner) {
        Congratulations.Instance.EndOfGame("Вітаємо гравця " + winner.nameInGame + " з перемогою. Він заробив " +
                                           winner.moneyAmount + " гривень та купив " + 
                                           winner.GetAllPlayerEnterprises(GamePlay.Instance.CurField).Count + " підприємств!");
    }
}
