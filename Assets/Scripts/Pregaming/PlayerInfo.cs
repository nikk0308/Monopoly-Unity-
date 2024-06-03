using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour 
{
    [SerializeField] private Image colorChip;
    [SerializeField] private TMP_Text namePlayer;
    [SerializeField] private TMP_Dropdown botType;
    [SerializeField] private Button edit;
    [SerializeField] private Button exit;

    public Image ColorChip => colorChip;
    public TMP_Text NamePlayer => namePlayer;
    public TMP_Dropdown BotType => botType;
    

    private void Start() {
        exit?.onClick.AddListener(() => PlayerInfoManager.Instance.DeletePlayerInfo(this));
        edit.onClick.AddListener(() => ScriptChooseColor.Instance.StartEditing(this));
        if (botType != null) {
            botType.options.Clear();
            foreach (var type in Constants.botsNames) {
                botType.options.Add(new TMP_Dropdown.OptionData(type));
            }
        }
    }
}

