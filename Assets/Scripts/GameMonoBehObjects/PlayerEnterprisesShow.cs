using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEnterprisesShow : MonoBehaviour
{
    [SerializeField] private Transform parent;
    [SerializeField] private EnterpriseInListShow enterpriseToInstantiate;
    
    [SerializeField] private Button close;
    [SerializeField] private Button finish;
    
    [SerializeField] private TMP_Text playerName;
    [SerializeField] private TMP_Text textToShow;
    [SerializeField] private TMP_Text moneyAmount;
    
    [SerializeField] private GameObject wrapperPriceFinishButton;
    [SerializeField] private TMP_Text textGainOrWaste;
    [SerializeField] private TMP_Text moneyGainOrWaste;

    private List<Enterprise> _curEnterprises;
    private HashSet<int> _enterpriseIndexes;
    private Player _player;
    private string _purposeGoal;
    public static PlayerEnterprisesShow Instance { get; private set; }     
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
    private void Start() {
        DeleteAllEnterprises();
        close.onClick.AddListener(CloseWindow);
    }

    public void FillWindow(Player player, string purposeGoal) {
        gameObject.SetActive(true);
        
        _player = player;
        _purposeGoal = purposeGoal;
        playerName.text = player.nameInGame;
        textGainOrWaste.text = Constants.TextMoneyEnterprisesWindow(_purposeGoal);
        moneyGainOrWaste.text = "0";
        moneyAmount.text = Convert.ToString(player.moneyAmount);
        textToShow.text = Constants.EnterprisesWindowText(_purposeGoal);
        finish.GetComponentInChildren<TMP_Text>().text = Constants.ButtonsEnterprisesWindow(_purposeGoal);
        ChooseOnClickFinish();
        
        if (_purposeGoal == "show") {
            FillWindowToJustShow();
        }
        if (_purposeGoal == "pawn") {
            FillWindowPawn();
        }
        if (_purposeGoal == "unpawn") {
            FillWindowUnPawn();
        }
        if (_purposeGoal == "build") {
            FillWindowBuildHome();
        }
    }

    public void FillWindowToJustShow() {
        wrapperPriceFinishButton.gameObject.SetActive(false);
        
        _curEnterprises = _player.GetAllPlayerEnterprises(GamePlay.Instance.CurField);
        foreach (var enterprise in _curEnterprises) {
            var newEnterprise = Instantiate(enterpriseToInstantiate, parent);
            newEnterprise.StartFill(enterprise, false);
        }
    }
    
    public void FillWindowPawn() {
        _enterpriseIndexes = new();
        wrapperPriceFinishButton.gameObject.SetActive(true);
        
        _curEnterprises = _player.GetPawnedOrNotPlayerEnterprises(GamePlay.Instance.CurField, false);
        foreach (var enterprise in _curEnterprises) {
            var newEnterprise = Instantiate(enterpriseToInstantiate, parent);
            newEnterprise.StartFill(enterprise, true);
        }

        if (_player.IsABot()) {
            PawnEnterprisesWithABot();
            finish.onClick.Invoke();
        }
    }
    
    public void FillWindowUnPawn() {
        _enterpriseIndexes = new();
        wrapperPriceFinishButton.gameObject.SetActive(true);
        
        _curEnterprises = _player.GetPawnedOrNotPlayerEnterprises(GamePlay.Instance.CurField, true);
        foreach (var enterprise in _curEnterprises) {
            var newEnterprise = Instantiate(enterpriseToInstantiate, parent);
            newEnterprise.StartFill(enterprise, true);
        }

        if (_player.IsABot()) {
            UnPawnEnterprisesWithABot();
            finish.onClick.Invoke();
        }
    }
    
    public void FillWindowBuildHome() {
        _enterpriseIndexes = new();
        wrapperPriceFinishButton.gameObject.SetActive(true);
        
        _curEnterprises = _player.GetFullIndustryWithoutNHotelsEnterprises(GamePlay.Instance.CurField);
        foreach (var enterprise in _curEnterprises) {
            var newEnterprise = Instantiate(enterpriseToInstantiate, parent);
            newEnterprise.StartFill(enterprise, true);
        }

        if (_player.IsABot()) {
            BuildHotelsWithABot();
            finish.onClick.Invoke();
        }
    }

    private void PawnListOfEnterprises() {
        int moneyToPayPlayer = 0;
        foreach (var index in _enterpriseIndexes) {
            moneyToPayPlayer += _curEnterprises[index].currentPriceOthersPay;
        }
        _player.moneyAmount += moneyToPayPlayer;
        
        foreach (var index in _enterpriseIndexes) {
            _curEnterprises[index].PawnInBank(GamePlay.Instance.CurField);
        }
    }

    private void CloseWindow() {
        DeleteAllEnterprises();
        gameObject.SetActive(false);
    }

    private void DeleteAllEnterprises() {
        foreach (Transform obj in parent) {
            Destroy(obj.gameObject);
        }
    }
    
    private void ChooseOnClickFinish() {
        finish.onClick.RemoveAllListeners();
        finish.onClick.AddListener(_purposeGoal switch {
            "pawn" => OnClickPawnButton,
            "unpawn" => OnClickUnPawnButton,
            "build" => OnClickBuildButton,
            _ => CloseWindow
        });
    }

    private void OnClickPawnButton() {
        PawnListOfEnterprises();
        CloseWindow();
        GameShowManager.Instance.UpdateAllPlayersInfo(GamePlay.Instance.Players, GamePlay.Instance.CurField);
        GameShowManager.Instance.PossibleReturnToGame();
    }

    private void OnClickUnPawnButton() {
        if (Convert.ToInt32(moneyGainOrWaste.text) > Convert.ToInt32(moneyAmount.text)) {
            PlayerInfoManager.Instance.ShowError("Грошей недостатньо!");
            return;
        }
        foreach (var index in _enterpriseIndexes) {
            _curEnterprises[index].UnPawnFromBank(GamePlay.Instance.CurField);
        }
        CloseWindow();
        GameShowManager.Instance.UpdateAllPlayersInfo(GamePlay.Instance.Players, GamePlay.Instance.CurField);
    }

    private void OnClickBuildButton() {
        if (Convert.ToInt32(moneyGainOrWaste.text) > Convert.ToInt32(moneyAmount.text)) {
            PlayerInfoManager.Instance.ShowError("Грошей недостатньо!");
            return;
        }
        foreach (var index in _enterpriseIndexes) {
            _curEnterprises[index].BuildHomeInEnterprise();
        }
        CloseWindow();
        GameShowManager.Instance.UpdateAllPlayersInfo(GamePlay.Instance.Players, GamePlay.Instance.CurField);
    }

    public void OnClickEnterprise(Enterprise enterprise, int enterpriseIndex, bool isSelect) {
        int money = GetMoneyEnterprise(enterprise);
        
        int curMoney = Convert.ToInt32(moneyGainOrWaste.text);
        if (isSelect) {
            _enterpriseIndexes.Add(enterpriseIndex);
            curMoney += money;
        }
        else {
            _enterpriseIndexes.Remove(enterpriseIndex);
            curMoney -= money;
        }
        moneyGainOrWaste.text = Convert.ToString(curMoney);
    }

    private int GetMoneyEnterprise(Enterprise enterprise) {
        return _purposeGoal switch {
            "pawn" => enterprise.currentPriceOthersPay,
            "unpawn" => enterprise.priceToBuy,
            "build" => enterprise.priceToBuildHotel,
        };
    }

    private void PawnEnterprisesWithABot() {
        int debtInMoney = _player.moneyAmount;
        Enterprise curEnterprise;
        List<Enterprise> enterprises = new List<Enterprise>(_curEnterprises);
        List<int> indexes = new();
        for (int i = 0; i < enterprises.Count; i++) {
            indexes.Add(i);
        }

        int enterpriseIndex = _player.WhichEnterprisePawnBot(enterprises);
        while (enterpriseIndex != -1 && debtInMoney < 0) {
            curEnterprise = enterprises[enterpriseIndex];
            OnClickEnterprise(curEnterprise, indexes[enterpriseIndex], true);
            enterprises.RemoveAt(enterpriseIndex);
            indexes.RemoveAt(enterpriseIndex);
            debtInMoney += GetMoneyEnterprise(curEnterprise);
            enterpriseIndex = _player.WhichEnterprisePawnBot(enterprises);
        }
    }

    private void UnPawnEnterprisesWithABot() {
        int playerMoneyLeft = _player.moneyAmount;
        Enterprise curEnterprise;
        List<Enterprise> enterprises = new List<Enterprise>(_curEnterprises);
        List<int> indexes = new();
        for (int i = 0; i < enterprises.Count; i++) {
            indexes.Add(i);
        }

        int enterpriseIndex = _player.WhichEnterpriseUnPawnBot(enterprises, playerMoneyLeft);
        while (enterpriseIndex != -1 && playerMoneyLeft > 0) {
            curEnterprise = enterprises[enterpriseIndex];
            OnClickEnterprise(curEnterprise, indexes[enterpriseIndex], true);
            enterprises.RemoveAt(enterpriseIndex);
            indexes.RemoveAt(enterpriseIndex);
            playerMoneyLeft -= GetMoneyEnterprise(curEnterprise);
            enterpriseIndex = _player.WhichEnterpriseUnPawnBot(enterprises, playerMoneyLeft);
        }
    }

    private void BuildHotelsWithABot() {
        int playerMoneyLeft = _player.moneyAmount;
        Enterprise curEnterprise;
        List<Enterprise> enterprises = new List<Enterprise>(_curEnterprises);
        List<int> indexes = new();
        for (int i = 0; i < enterprises.Count; i++) {
            indexes.Add(i);
        }

        int enterpriseIndex = _player.WhichEnterpriseBuildHotelBot(enterprises, playerMoneyLeft);
        while (enterpriseIndex != -1 && playerMoneyLeft > 0) {
            curEnterprise = enterprises[enterpriseIndex];
            OnClickEnterprise(curEnterprise, indexes[enterpriseIndex], true);
            enterprises.RemoveAt(enterpriseIndex);
            indexes.RemoveAt(enterpriseIndex);
            playerMoneyLeft -= GetMoneyEnterprise(curEnterprise);
            enterpriseIndex = _player.WhichEnterpriseBuildHotelBot(enterprises, playerMoneyLeft);
        }
    }
}
