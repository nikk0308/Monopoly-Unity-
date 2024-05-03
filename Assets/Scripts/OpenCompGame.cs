using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCompGame : MonoBehaviour
{
    [SerializeField] private bool isWithBots;

    public void OpenCompGameObject() {
        PlayerInfoManager.Instance.isWithBots = isWithBots;
        PlayerInfoManager.Instance.gameObject.SetActive(true);
        if (!isWithBots) {
            return;
        }
        PlayerInfoManager.Instance.CreateOwner();
        ChangeInfo first = PlayerInfoManager.Instance.PlayerInfo[^1];
        first.playerName.text = "Грак";
    }
}
