using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShow : MonoBehaviour {
    
    [SerializeField] private Image chipColor;
    [SerializeField] private TMP_Text namePlayer;
    [SerializeField] private TMP_Text moneyAmount;
    [SerializeField] private TMP_Text normalEnterprises;
    [SerializeField] private TMP_Text pawnedEnterprises;
    
    [SerializeField] private GameObject lockPlayer;
    [SerializeField] private Button playerClick;
    
    public string NamePlayer => namePlayer.text;
    public Color ColorPlayer => chipColor.color;

    private Player _player;
    
    public PlayerShow(Color playerColor, string playerName, Player player) {
        SetStartInfo(playerColor, playerName, player);
    }

    private void Start() {
        playerClick.onClick.AddListener(OnPlayerClicked);
    }
    
    public void SetStartInfo(Color playerColor, string playerName, Player player) {
        chipColor.color = playerColor;
        namePlayer.text = playerName;
        _player = player;
        UpdateInfo(0, 0, 0);
        lockPlayer.gameObject.SetActive(false);
    }

    public void UpdateInfo(int moneyAmount, int enterprisesAmount, int pawnedEnterprisesAmount) {
        this.moneyAmount.text = Convert.ToString(moneyAmount);
        normalEnterprises.text = Convert.ToString(enterprisesAmount);
        pawnedEnterprises.text = Convert.ToString(pawnedEnterprisesAmount);
    }

    public void SetMoneyAmount(int moneyAmount) {
        this.moneyAmount.text = Convert.ToString(moneyAmount);
    }
    public void SetNormalEnterprises(int enterprisesAmount) {
        normalEnterprises.text = Convert.ToString(enterprisesAmount);
    }
    public void SetPawnedEnterprises(int enterprisesAmount) {
        pawnedEnterprises.text = Convert.ToString(enterprisesAmount);
    }

    private void OnPlayerClicked() {
        PlayerEnterprisesShow.Instance.FillWindow(_player, "show");
    }

    public void PlayerIsBankrupt() {
        lockPlayer.gameObject.SetActive(true);
    }
}
