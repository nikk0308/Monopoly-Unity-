using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GamePlay : MonoBehaviour
{
    public static GamePlay Instance { get; private set; }     
    private Field field;
    private readonly int indexOfEndOfCountry;
    private readonly int indexOfEndOfArray;
    private readonly int indexOfWorkCell;
    private readonly int enterOnArrayInAnother;


    public static readonly int salary = 100;
    public static readonly int startCapital = 300;

    public GamePlay() {
        RecreateField();
        indexOfEndOfCountry = field.specialIndexesByCellNames["ExitChance"];
        indexOfEndOfArray = field.fieldArrays[0].Length - 1;
        indexOfWorkCell = field.specialIndexesByCellNames["Work"];
        enterOnArrayInAnother = indexOfWorkCell;
    }

    public void RecreateField() {
        field = new Field();
    }

    public void StartGame(Player[] players) {
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

            messageToPrint = field.TakeCardByPlayerPos(curPlayer)
                .DoActionIfStayed(field, curPlayer, out isNextMoveNeed);
            JustOutput.PrintText(messageToPrint);

            if (isNextMoveNeed) {
                PlayerTurnWithDice(curPlayer);
                JustOutput.PrintText(OutputPhrases.PlayerMovedTo(curPlayer, field));
                messageToPrint = field.TakeCardByPlayerPos(curPlayer).DoActionIfArrived(field, curPlayer);
                JustOutput.PrintText(messageToPrint);
            }

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
            Interactive.PressEnter();
        }
    }

    private void PlayerTurnWithDice(Player player) {
        int curPlayerArr = player.positionInField.arrayIndex;
        int curPlayerCell = player.positionInField.cellIndex;

        JustOutput.PrintText(OutputPhrases.TextRollDice(player));
        if (!player.IsABot()) {
            Interactive.PressEnter();
        }

        int randTurnsAmount = Constants.RollDice();
        JustOutput.PrintText(OutputPhrases.TextDiceNumber(randTurnsAmount));

        int newPlayerCell = curPlayerCell + randTurnsAmount;

        if (curPlayerCell < indexOfWorkCell && newPlayerCell >= indexOfWorkCell) {
            JustOutput.PrintText(OutputPhrases.TextGainSalary(player, salary));
            player.moneyAmount += salary;
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
    
    
    
    
    
    
    
    
    
    
    private void Awake() 
    {
        if (Instance != null && Instance != this) {
            Destroy(this);
        }
        else {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
