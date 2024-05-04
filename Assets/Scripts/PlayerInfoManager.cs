using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlayerInfoManager : MonoBehaviour 
{
    [SerializeField] private PlayerInfo objectBotInfo;
    [SerializeField] private PlayerInfo objectPlayerInfo;
    [SerializeField] private PlayerInfo objectOwnerInfo;
    
    [SerializeField] private Transform parent;
    [SerializeField] private Button create;
    [SerializeField] private Button close;
    [SerializeField] private Button startGame;
    [SerializeField] private TMP_Text title;

    [SerializeField] private Image errorWindow;
    [SerializeField] private TMP_Text errorText;
    [SerializeField] private Button closeError;
    public bool isWithBots;

    private List<PlayerInfo> _playerInfo = new();
    private List<string> _defaultNames = new();

    public List<PlayerInfo> PlayerInform => _playerInfo;
    public static PlayerInfoManager Instance { get; private set; }     
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
        close.onClick.AddListener(CloseWindow);
        startGame.onClick.AddListener(StartGame);
        closeError.onClick.AddListener(CloseError);
        NamesFill();
        CloseWindow();
    }
    public void OnEnable()
    {
        create.onClick.RemoveAllListeners();
        if (isWithBots) {
            title.text = "Гра з комп'ютером";
            create.onClick.AddListener(CreateBot);
        }
        else {
            title.text = "Гра з друзями";
            create.onClick.AddListener(CreatePlayer);
        }
    }
    
    private void CreateBot() {
        var botInfo = Instantiate(objectBotInfo, parent);
        SetDefaultPlayerValue(botInfo);
    }
    
    private void CreatePlayer() {
        var playerInfo = Instantiate(objectPlayerInfo, parent);
        SetDefaultPlayerValue(playerInfo);
    }
    
    public void CreateOwner() {
        var ownerInfo = Instantiate(objectOwnerInfo, parent);
        SetDefaultPlayerValue(ownerInfo);
        ownerInfo.NamePlayer.text = "Грак";
    }

    private void SetDefaultPlayerValue(PlayerInfo newPlayer) {
        newPlayer.ColorChip.color = new Color(1f, 1f, 1f);
        newPlayer.NamePlayer.text = RandUniqueName();
        _playerInfo.Add(newPlayer);
        if (_playerInfo.Count >= Constants.MaxPlayersAmount) {
            create.enabled = false;
        }
    }

    private void CloseWindow() {
        foreach (Transform child in parent) {
            Destroy(child.gameObject);
        }
        _playerInfo = new();
        create.enabled = true;
        gameObject.SetActive(false);
    }

    public void DeletePlayerInfo(PlayerInfo playerInfo) {
        var index = playerInfo.transform.GetSiblingIndex();
        _playerInfo.RemoveAt(index);
        Destroy(playerInfo.gameObject);
        if (_playerInfo.Count < Constants.MaxPlayersAmount) {
            create.enabled = true;
        }
    }

    private void StartGame() {
        if (_playerInfo.Count < Constants.MinPlayersAmount) {
            ShowError("Недостатня кількість гравців");
            return;
        }

        string output = "\n";
        for (int i = 0; i < _playerInfo.Count; i++) {
            output += _playerInfo[i].NamePlayer.text + " | " + _playerInfo[i].ColorChip.color + " | " +
                      (_playerInfo[i].BotType == null ? "немає" : Convert.ToString(_playerInfo[i].BotType.value)) + "\n";
        }
        Debug.Log(output);
    }

    public void ShowError(string errorText) {
        this.errorText.text = errorText;
        errorWindow.gameObject.SetActive(true);
    }

    public bool IsPlayerNameAlreadyExist(string playerName, int nameIndex = -1) {
        for (int i = 0; i < _playerInfo.Count; i++) {
            if (playerName == _playerInfo[i].NamePlayer.text && nameIndex != i) {
                return true;
            }
        }
        foreach (var player in _playerInfo) {
        }
        return false;
    }

    private void CloseError() {
        errorWindow.gameObject.SetActive(false);
    }
    private void NamesFill() {
        var fileNames = Resources.Load<TextAsset>("text_info/names_for_bots/normal_names").ToString();
        int startIndex = 0;
        int findIndex = fileNames.IndexOf('\n', startIndex);
        while (findIndex != -1) {
            _defaultNames.Add(fileNames.Substring(startIndex, findIndex - startIndex));
            startIndex = findIndex + 1;
            findIndex = fileNames.IndexOf('\n', startIndex);
        }
    }

    private string RandUniqueName() {
        string nameToReturn;
        do {
            nameToReturn = _defaultNames[Constants.Rand.Next(0, _defaultNames.Count)];
        } while (IsPlayerNameAlreadyExist(nameToReturn));
        return nameToReturn;
    }
}