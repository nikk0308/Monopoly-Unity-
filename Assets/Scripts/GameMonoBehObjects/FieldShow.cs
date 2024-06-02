using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FieldShow : MonoBehaviour
{
    
    [SerializeField] private Transform field0;
    [SerializeField] private Transform field1;
    [SerializeField] private CardShow startCellShow;
    
    [SerializeField] private Image countryFlag0;
    [SerializeField] private Image countryFlag1;

    [SerializeField] private GameObject wrapperInfo1;
    [SerializeField] private TMP_Text textInfo1;
    [SerializeField] private GameObject wrapperInfo2;
    [SerializeField] private TMP_Text titleInfo2;
    [SerializeField] private TMP_Text textInfo2;
    [SerializeField] private GameObject wrapperButtons;
    [SerializeField] private Button yesInfo2;
    [SerializeField] private Button noInfo2;
    
    private readonly List<CardShow> _field0Cards = new();
    private readonly List<CardShow> _field1Cards = new();

    private bool isYesLastTapped;

    public GameObject WrapperInfo2 => wrapperInfo2;

    private void Awake() {
        foreach (Transform card in field0) {
            _field0Cards.Add(card.GetComponent<CardShow>());
        }
        foreach (Transform card in field1) {
            _field1Cards.Add(card.GetComponent<CardShow>());
        }
        yesInfo2.onClick.AddListener(OnClickButtonYes);
        noInfo2.onClick.AddListener(OnClickButtonNo);
    }

    private void OnClickButtonYes() {
        GamePlay.Instance.ContinueAfterSelectedAns(true);
    }

    private void OnClickButtonNo() {
        GamePlay.Instance.ContinueAfterSelectedAns(false);
    }

    public void SetStartValues(Field field) {
        Card[] firstArray = field.fieldArrays[0];
        Card[] secondArray = field.fieldArrays[1];

        for (int i = 0; i < Field.arrayLength; i++) {
            if (firstArray[i] is Enterprise curEnterprise) {
                _field0Cards[i].info.SetStartInfo(curEnterprise.title, curEnterprise.industry.color,
                    curEnterprise.priceOthersPayLevel1,
                    curEnterprise.priceOthersPayLevel2, curEnterprise.priceOthersPayLevel3);
            }
        }
        
        for (int i = 0; i < Field.arrayLength; i++) {
            if (secondArray[i] is Enterprise curEnterprise) {
                _field1Cards[i].info.SetStartInfo(curEnterprise.title, curEnterprise.industry.color,
                    curEnterprise.priceOthersPayLevel1,
                    curEnterprise.priceOthersPayLevel2, curEnterprise.priceOthersPayLevel3);
            }
        }
        
        ChangeFieldFlag(field.countriesArray[0], 0);
        ChangeFieldFlag(field.countriesArray[1], 1);
    }

    public void Tresh() {
        int amount = 16;
        foreach (CardShow card in _field0Cards) {
            card.RemoveAllPlayersFromCard();
            card.info?.SetStartInfo("Givenchy", Color.white, 120, 240, 360);
            card.info?.SetOwnerColorToNone();
            card.info?.SetCurrentActivePriceToNone();
        }
        for (int i = 0; i < amount; i++) {
            int randNum = Constants.Rand.Next(21);
            _field0Cards[randNum].info?.SetStartInfo("OOOOOOOOOOOOOOO", Constants.RandColor(), Constants.Rand.Next(3000), 
                Constants.Rand.Next(3000), Constants.Rand.Next(3000));
            _field0Cards[randNum].info?.SetCurrentActivePrice(Constants.Rand.Next(1, 4));
        }
        for (int i = 0; i < amount; i++) {
            int randNum = Constants.Rand.Next(21);
            _field0Cards[randNum].info?.SetOwnerColor(Constants.RandColor());
        }
        for (int i = 0; i < amount; i++) {
            int randNum = Constants.Rand.Next(21);
            _field0Cards[randNum].MovePlayerOnCard(Constants.RandColor());
        }

        ChangeFieldFlag("Germany", Constants.Rand.Next(2));
    }

    public CardShow GetCardShowByPosition(Position position) {
        if (position == null) {
            return startCellShow;
        }

        position = Position.CreatePrePosition(position);
        if (position.cellIndex == -1) {
            position.cellIndex = Field.specialIndexesByCellNames["ExitChance"];
        }

        CardShow cardAns;
        switch(position.arrayIndex)
        {
            case 0:
                cardAns = _field0Cards[position.cellIndex];
                break;
            default:
                cardAns = _field1Cards[position.cellIndex];
                break;
        }
        return cardAns;
    }

    private void ChangeFieldFlag(string countryName, int fieldIndex) {
        ChangeImageByFlagName(fieldIndex == 0 ? countryFlag0 : countryFlag1, Constants.flagNamesByCountryNames[countryName]);
    }
    
    private void ChangeImageByFlagName(Image image, string flagName) {
        string flagsFolder = "Flags/";
        Texture2D texture = Resources.Load<Texture2D>(flagsFolder + flagName);
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        image.sprite = sprite;
    }
    
    public void ShowInfoBlock1(bool isShow) {
        wrapperInfo1.SetActive(isShow);
    }
    
    public void ShowInfoBlock2(bool isShow) {
        wrapperInfo2.SetActive(isShow);
    }
    
    public void ShowButtonsBlock2(bool isShow) {
        wrapperButtons.SetActive(isShow);
    }

    public void SetTextInfo1(string textToSet) {
        textInfo1.text = textToSet;
    }

    public void SetAdditionTextInfo1(string textToAdd) {
        if (textToAdd.Length > 0) {
            if (textInfo1.text.Length > 0) {
                textInfo1.text += "\n\n";
            }
            textInfo1.text += textToAdd;
        }
    }

    public void SetTextInfo2(string textToSet) {
        textInfo2.text = textToSet;
    }

    public void SetTitleInfo2(string textToSet) {
        titleInfo2.text = textToSet;
    }

    public void AutoAddText(ref string textForBlock1, ref string textForBlock2, string curText) {
        if (textForBlock2.Length > 0) {
            if (textForBlock1.Length > 0) {
                textForBlock1 += "\n\n";
            }
            textForBlock1 += textForBlock2;
        }
        textForBlock2 = curText;
    }
}
