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
    [SerializeField] private TMP_Text title;
    public bool isWithBots;

    private PlayersList _playerInfo = new();
    private int maxPlayersAmount = 6;

    public PlayersList PlayerInfo => _playerInfo;
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
        if (_playerInfo.Count() >= maxPlayersAmount) {
            create.enabled = false;
        }
    }
    
    private void CreatePlayer() {
        var playerInfo = Instantiate(objectPlayerInfo, parent);
        SetDefaultPlayerValue(playerInfo.NamePlayer, playerInfo.ColorChip);
        if (_playerInfo.Count() >= maxPlayersAmount) {
            create.enabled = false;
        }
    }
    
    public void CreateOwner() {
        var ownerInfo = Instantiate(objectOwnerInfo, parent);
        SetDefaultPlayerValue(ownerInfo.NamePlayer, ownerInfo.ColorChip);
    }

    private void SetDefaultPlayerValue(TMP_Text namePlayer, Image colorPlayer) {
        colorPlayer.color = new Color(1f, 1f, 1f);
        namePlayer.text = "Гравець №" + Convert.ToString(_playerInfo.Count() + 1);
        _playerInfo.AddElement(namePlayer, colorPlayer);
    }

    private void CloseWindow() {
        foreach (Transform child in parent) {
            Destroy(child.gameObject);
        }
        _playerInfo.DeleteAll();
        gameObject.SetActive(false);
    }

    public void DeletePlayerInfo(BotInfo botInfo) {
        var index = botInfo.transform.GetSiblingIndex();
        _playerInfo.RemoveAt(index);
        Destroy(botInfo.gameObject);
        if (_playerInfo.Count() < maxPlayersAmount) {
            create.enabled = true;
        }
    }
    public void DeletePlayerInfo(PlayerInfo botInfo) {
        var index = botInfo.transform.GetSiblingIndex();
        _playerInfo.RemoveAt(index);
        Destroy(botInfo.gameObject);
        if (_playerInfo.Count() < maxPlayersAmount) {
            create.enabled = true;
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

