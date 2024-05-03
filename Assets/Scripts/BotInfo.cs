using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BotInfo : MonoBehaviour 
{
    [SerializeField] private Image colorChip;
    [SerializeField] private TMP_Text namePlayer;
    [SerializeField] private TMP_Dropdown botType;
    [SerializeField] private Button edit;
    [SerializeField] private Button exit;

    public Image ColorChip => colorChip;
    public TMP_Text NamePlayer => namePlayer;
    

    private void Start() {
        exit.onClick.AddListener(() => PlayerInfoManager.Instance.DeletePlayerInfo(this));
        edit.onClick.AddListener(() => ScriptChooseColor.Instance.StartEditing(new ChangeInfo(NamePlayer, ColorChip)));
    }

    public void BotTypeChanged() {
        var index = transform.GetSiblingIndex();
        int newPickedType = botType.value;
        PlayerInfoManager.Instance.BotsIndexes[index] = newPickedType;
    }
}

