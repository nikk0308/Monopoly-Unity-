using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ScriptChooseColor : MonoBehaviour, IPointerClickHandler
{
    public Image imageToGainColor;
    public Image imageToShow;

    public void OnPointerClick(PointerEventData eventData) {
        Vector2 localCursor;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(imageToGainColor.rectTransform,
            eventData.position, eventData.pressEventCamera, out localCursor);

        var imageRect = imageToGainColor.rectTransform.rect;
        Vector2 normalizedPosition = new Vector2((localCursor.x + imageRect.width * 0.5f) / imageRect.width,
            (localCursor.y + imageRect.height * 0.5f) / imageRect.height);

        Texture2D texture = imageToGainColor.sprite.texture;

        Vector2 texCoord = new Vector2(normalizedPosition.x * texture.width, normalizedPosition.y * texture.height);

        Color color = texture.GetPixel((int)texCoord.x, (int)texCoord.y);
        ChangeImageColor(imageToShow, color);

    }

    private void ChangeImageColor(Image image, Color color) {
        image.color = color;
    }
}