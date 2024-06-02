using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnterpriseInfo : MonoBehaviour {
    [SerializeField] private Image enterpriseColor;
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text priceLevel1;
    [SerializeField] private TMP_Text priceLevel2;
    [SerializeField] private TMP_Text priceLevel3;
    
    [SerializeField] public Image ownerColor;
    [SerializeField] public Image priceColor1;
    [SerializeField] public Image priceColor2;
    [SerializeField] public Image priceColor3;
    
    [SerializeField] public GameObject pawnedImage;
    [SerializeField] public TMP_Text pawnedTurns;

    private void Start() {
        SetPricesColorsToDefault();
        ownerColor.color = Constants.DefaultOutlineColor;
    }

    public void SetStartInfo(string enterTitle, Color enterColor, int enterPriceLevel1, int enterPriceLevel2, int enterPriceLevel3) {
        title.text = enterTitle;
        enterpriseColor.color = enterColor;
        priceLevel1.text = Convert.ToString(enterPriceLevel1);
        priceLevel2.text = Convert.ToString(enterPriceLevel2);
        priceLevel3.text = Convert.ToString(enterPriceLevel3);
        PawnEnterprise(false);
        SetOwnerColorToNone();
        SetCurrentActivePriceToNone();
    }

    public void SetOwnerColor(Color playerColor) {
        ownerColor.color = playerColor;
    }

    public void SetOwnerColorToNone() {
        ownerColor.color = Constants.DefaultOutlineColor;
    }

    public void SetCurrentActivePrice(int priceLevel) {
        SetPricesColorsToDefault();
        switch (priceLevel) {
            case 1:
                priceColor1.color = Constants.CurrentPriceColor;
                break;
            case 2:
                priceColor2.color = Constants.CurrentPriceColor;
                break;
            case 3:
                priceColor3.color = Constants.CurrentPriceColor;
                break;
        }
    }

    public void SetCurrentActivePriceToNone() {
        SetCurrentActivePrice(-1);
    }

    public void SetPricesColorsToDefault() {
        priceColor1.color = Constants.PrimaryPriceColor;
        priceColor2.color = Constants.PrimaryPriceColor;
        priceColor3.color = Constants.PrimaryPriceColor;
    }

    public void PawnEnterprise(bool isPawn, int turnsToDisappear = 0) {
        pawnedImage.gameObject.SetActive(isPawn);
        SetTurnsToDisappear(turnsToDisappear);
    }

    public void SetTurnsToDisappear(int turnsToDisappear) {
        pawnedTurns.text = Convert.ToString(turnsToDisappear);
    }
}
