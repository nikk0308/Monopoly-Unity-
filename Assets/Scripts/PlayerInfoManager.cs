using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerInfoManager : MonoBehaviour 
{
    [SerializeField] private BotInfo objectBotInfo;
    [SerializeField] private PlayerInfo objectPlayerInfo;
    [SerializeField] private PlayerInfo objectOwnerInfo;
    
    [SerializeField] private Transform parent;
    [SerializeField] private Button create;
    [SerializeField] private Button close;
    [SerializeField] private Button startGame;
    [SerializeField] private TMP_Text title;
    public bool isWithBots;

    private List<ChangeInfo> _playerInfo = new();
    private List<int> _botsIndexes = new();
    private int maxPlayersAmount = 6;

    public List<ChangeInfo> PlayerInfo => _playerInfo;
    public List<int> BotsIndexes => _botsIndexes;
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
        SetDefaultPlayerValue(botInfo.NamePlayer, botInfo.ColorChip);
        if (_playerInfo.Count >= maxPlayersAmount) {
            create.enabled = false;
        }
        _botsIndexes.Add(0);
    }
    
    private void CreatePlayer() {
        var playerInfo = Instantiate(objectPlayerInfo, parent);
        SetDefaultPlayerValue(playerInfo.NamePlayer, playerInfo.ColorChip);
        if (_playerInfo.Count >= maxPlayersAmount) {
            create.enabled = false;
        }
        _botsIndexes.Add(-1);
    }
    
    public void CreateOwner() {
        var ownerInfo = Instantiate(objectOwnerInfo, parent);
        SetDefaultPlayerValue(ownerInfo.NamePlayer, ownerInfo.ColorChip);
        _botsIndexes.Add(-1);
    }

    private void SetDefaultPlayerValue(TMP_Text namePlayer, Image colorPlayer) {
        colorPlayer.color = new Color(1f, 1f, 1f);
        namePlayer.text = "Гравець №" + Convert.ToString(_playerInfo.Count + 1);
        AddElementToList(namePlayer, colorPlayer);
    }

    private void CloseWindow() {
        foreach (Transform child in parent) {
            Destroy(child.gameObject);
        }
        _playerInfo = new();
        gameObject.SetActive(false);
    }

    public void DeletePlayerInfo(BotInfo botInfo) {
        var index = botInfo.transform.GetSiblingIndex();
        _playerInfo.RemoveAt(index);
        _botsIndexes.RemoveAt(index);
        Destroy(botInfo.gameObject);
        if (_playerInfo.Count < maxPlayersAmount) {
            create.enabled = true;
        }
    }
    public void DeletePlayerInfo(PlayerInfo botInfo) {
        var index = botInfo.transform.GetSiblingIndex();
        _playerInfo.RemoveAt(index);
        Destroy(botInfo.gameObject);
        if (_playerInfo.Count < maxPlayersAmount) {
            create.enabled = true;
        }
    }
    
    public void AddElementToList(TMP_Text nameInfo, Image colorInfo) {
        ChangeInfo changeInfo = new ChangeInfo(nameInfo, colorInfo);
        _playerInfo.Add(changeInfo);
    }

    private void StartGame() {
        for (int i = 0; i < _playerInfo.Count; i++) {
            Debug.Log(_playerInfo[i].playerName.text + " | " + _playerInfo[i].chipColor.color + " | " + _botsIndexes[i]);
        }
    }
}

public class PlayersList {
    private List<ChangeInfo> list = new();
    
    public void AddElement(TMP_Text nameInfo, Image colorInfo) {
        ChangeInfo changeInfo = new ChangeInfo(nameInfo, colorInfo);
        list.Add(changeInfo);
    }

    public void RemoveAt(int index) {
        list.RemoveAt(index);
    }

    public ChangeInfo GetAt(Index index) {
        return list[index];
    }

    public int Count() {
        return list.Count;
    }

    public void DeleteAll() {
        list = new();
    }
}

