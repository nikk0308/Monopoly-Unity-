using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScriptChooseColor : MonoBehaviour, IPointerClickHandler {
    [SerializeField] private Image imageToGainColor;
    [SerializeField] private Image imageToShow;
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private Button save;

    private ChangeInfo _objectToEdit;
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
        save.onClick.AddListener(Save);
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
        _objectToEdit.chipColor.color = imageToShow.color;
        _objectToEdit.playerName.text = nameInputField.text;
        gameObject.SetActive(false);
    }
    
    public void StartEditing(ChangeInfo changeInfo) {
        _objectToEdit = changeInfo;
        gameObject.SetActive(true);
        imageToShow.color = changeInfo.chipColor.color;
        nameInputField.text = changeInfo.playerName.text;
    }
}

public class ChangeInfo {
    public TMP_Text playerName;
    public Image chipColor;
    public ChangeInfo(TMP_Text playerName, Image chipColor) {
        this.playerName = playerName;
        this.chipColor = chipColor;
    }
}