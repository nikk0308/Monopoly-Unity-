using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    
    [SerializeField] private Image playGameWindow;
    public bool isWithBots;

    private List<PlayerInfo> _playerInfo = new();
    private List<string> _defaultNames = new();

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
        // Here error
        NamesFill();
        close.onClick.AddListener(CloseWindow);
        startGame.onClick.AddListener(StartGame);
        closeError.onClick.AddListener(CloseError);
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
        ClearPlayerColorAndName(playerInfo);
        ScriptChooseColor.Instance.StartEditing(playerInfo);
    }
    
    public void CreateOwner() {
        var ownerInfo = Instantiate(objectOwnerInfo, parent);
        SetDefaultPlayerValue(ownerInfo);
        ClearPlayerColorAndName(ownerInfo);
        ScriptChooseColor.Instance.StartEditing(ownerInfo);
    }

    private void SetDefaultPlayerValue(PlayerInfo newPlayer) {
        newPlayer.ColorChip.color = RandUniqueColor();
        newPlayer.NamePlayer.text = RandUniqueName();
        _playerInfo.Add(newPlayer);
        if (_playerInfo.Count >= Constants.MaxPlayersAmount) {
            create.enabled = false;
        }
    }

    private void ClearPlayerColorAndName(PlayerInfo player) {
        player.ColorChip.color = Color.white;
        player.NamePlayer.text = "";
    }

    private Color RandUniqueColor() {
        Color colorToReturn;
        do {
            colorToReturn = Constants.RandColor();
        } while (IsSimilarColorExist(colorToReturn));
        return colorToReturn;
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
        CloseWindow();
        GameShowManager.Instance.EnableWindow();
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
        return false;
    }
    
    public bool IsSimilarColorExist(Color playerColor, int nameIndex = -1) {
        for (int i = 0; i < _playerInfo.Count; i++) {
            if (EvklidColorDistance(playerColor, _playerInfo[i].ColorChip.color) < Constants.PlayersColorsSimilarityDegree
                && nameIndex != i) {
                return true;
            }
        }
        return false;
    }

    private double EvklidColorDistance(Color first, Color second) {
        return Math.Sqrt(Math.Pow(first.r - second.r, 2) + Math.Pow(first.g - second.g, 2) + Math.Pow(first.b - second.b, 2));
    }
        
    private void CloseError() {
        errorWindow.gameObject.SetActive(false);
    }
    private void NamesFill() {
    //     string fileLocation = "Assets/Resources/text_info/names_for_bots/normal_names.txt";
    //     string[] names = File.ReadAllLines(fileLocation);
    //     foreach (var name in names) {
    //         _defaultNames.Add(name);
    //     }
    //     
        string fileLocation = "text_info/names_for_bots/normal_names";
        TextAsset textAsset = Resources.Load<TextAsset>(fileLocation);

        if (textAsset != null) {
            string[] names = textAsset.text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var name in names) {
                _defaultNames.Add(name);
            }
        } else {
            Debug.LogError("File not found at: " + fileLocation);
        }
    }

    private string RandUniqueName() {
        string nameToReturn;
        do {
            // Here error
            nameToReturn = _defaultNames[Constants.Rand.Next(0, _defaultNames.Count)];
        } while (IsPlayerNameAlreadyExist(nameToReturn));
        return nameToReturn;
    }
}