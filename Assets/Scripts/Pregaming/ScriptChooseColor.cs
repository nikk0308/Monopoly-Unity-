using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScriptChooseColor : MonoBehaviour, IPointerClickHandler {
    [SerializeField] private Image imageToGainColor;
    [SerializeField] private Image imageToShow;
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private Button save;
    [SerializeField] private Button close;

    private PlayerInfo _objectToEdit;
    public static ScriptChooseColor Instance { get; private set; }     
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
        nameInputField.characterLimit = Constants.MaxNameLength;
        save.onClick.AddListener(Save);
        close.onClick.AddListener(CloseWindow);
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.pointerCurrentRaycast.gameObject != imageToGainColor.gameObject) {
            return;
        }
        RectTransformUtility.ScreenPointToLocalPointInRectangle(imageToGainColor.rectTransform,
            eventData.position, eventData.pressEventCamera, out Vector2 localCursor);

        var imageRect = imageToGainColor.rectTransform.rect;
        Vector2 normalizedPosition = new Vector2((localCursor.x + imageRect.width * 0.5f) / imageRect.width,
            (localCursor.y + imageRect.height * 0.5f) / imageRect.height);

        Texture2D texture = imageToGainColor.sprite.texture;

        Vector2 texCoord = new Vector2(normalizedPosition.x * texture.width, normalizedPosition.y * texture.height);

        Color color = texture.GetPixel((int)texCoord.x, (int)texCoord.y);
        imageToShow.color = color;
    }


    private void Save() {
        if (!IsInputCorrect()) {
            return;
        }
        _objectToEdit.ColorChip.color = imageToShow.color;
        _objectToEdit.NamePlayer.text = nameInputField.text;
        close.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    private bool IsInputCorrect() {
        if (nameInputField.text.Length < Constants.MinNameLength) {
            PlayerInfoManager.Instance.ShowError("Ім'я занадто мале");
            return false;
        }
        if (PlayerInfoManager.Instance.IsPlayerNameAlreadyExist(nameInputField.text, _objectToEdit.transform.GetSiblingIndex())) {
            PlayerInfoManager.Instance.ShowError("Таке ім'я вже існує");
            return false;
        }
        if (PlayerInfoManager.Instance.IsSimilarColorExist(imageToShow.color, _objectToEdit.transform.GetSiblingIndex())) {
            PlayerInfoManager.Instance.ShowError("Схожий колір вже існує");
            return false;
        }
        return true;
    }
    
    public void StartEditing(PlayerInfo changeInfo) {
        _objectToEdit = changeInfo;
        gameObject.SetActive(true);
        imageToShow.color = changeInfo.ColorChip.color;
        nameInputField.text = changeInfo.NamePlayer.text;
        if (nameInputField.text.Length == 0) {
            close.gameObject.SetActive(false);
        }
    }

    private void CloseWindow() {
        gameObject.SetActive(false);
    }
}