using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnterpriseInListShow : MonoBehaviour
{
    [SerializeField] private Image enterpriseColor;
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text priceLevel1;
    [SerializeField] private TMP_Text priceLevel2;
    [SerializeField] private TMP_Text priceLevel3;
    
    [SerializeField] public Image isSelectedImage;
    [SerializeField] public Image priceColor1;
    [SerializeField] public Image priceColor2;
    [SerializeField] public Image priceColor3;
    
    [SerializeField] public Button enterpriseClick;
    [SerializeField] public GameObject pawnedImage;
    [SerializeField] public TMP_Text pawnedTurns;

    private bool _isSelected;
    private Enterprise _enterprise;
    
    void Start()
    {
        enterpriseClick.onClick.AddListener(OnEnterpriseClicked);
    }

    public void StartFill(Enterprise enterprise, bool isClickable) {
        _enterprise = enterprise;
        enterpriseColor.color = _enterprise.industry.color;
        title.text = _enterprise.title;

        priceLevel1.text = Convert.ToString(_enterprise.priceOthersPayLevel1);
        priceLevel2.text = Convert.ToString(_enterprise.priceOthersPayLevel2);
        priceLevel3.text = Convert.ToString(_enterprise.priceOthersPayLevel3);

        SetCurrentActivePrice(FindCurrentPriceLevel());
        SelectEnterpriseColor(false);
        PawnEnterprise(_enterprise.IsPawned(), _enterprise.turnsToDisappearIfPawned);
        enterpriseClick.interactable = isClickable;
    }

    private void SelectEnterpriseColor(bool isSelect) {
        isSelectedImage.color = isSelect ? Constants.SelectedOutlineColor : Constants.DefaultOutlineColor;
    }

    private void SetCurrentActivePrice(int priceLevel) {
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

    private void SetPricesColorsToDefault() {
        priceColor1.color = Constants.PrimaryPriceColor;
        priceColor2.color = Constants.PrimaryPriceColor;
        priceColor3.color = Constants.PrimaryPriceColor;
    }

    private int FindCurrentPriceLevel() {
        if (_enterprise.currentPriceOthersPay == _enterprise.priceOthersPayLevel1) {
            return 1;
        }
        if (_enterprise.currentPriceOthersPay == _enterprise.priceOthersPayLevel2) {
            return 2;
        }
        return 3;
    }

    private void OnEnterpriseClicked() {
        _isSelected = !_isSelected;
        SelectEnterpriseColor(_isSelected);
        PlayerEnterprisesShow.Instance.OnClickEnterprise(_enterprise, transform.GetSiblingIndex(), _isSelected);
    }
    
    public void PawnEnterprise(bool isPawn, int turnsToDisappear = 0) {
        pawnedImage.gameObject.SetActive(isPawn);
        SetTurnsToDisappear(turnsToDisappear);
    }

    public void SetTurnsToDisappear(int turnsToDisappear) {
        pawnedTurns.text = Convert.ToString(turnsToDisappear);
    }
}
