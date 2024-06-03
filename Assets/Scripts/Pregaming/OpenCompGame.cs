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
    }
}
