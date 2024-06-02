using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardShow : MonoBehaviour {
    [SerializeField] private Transform playersPlace;
    [SerializeField] private Chip chipToInstantiate;
    [SerializeField] public EnterpriseInfo info;

    private List<Chip> chipList = new();

    private void Start() {
        RemoveAllPlayersFromCard();
    }
    public void MovePlayerOnCard(Color playerColor) {
        var newChip = Instantiate(chipToInstantiate, playersPlace);
        newChip.ColorChange(playerColor);
        chipList.Add(newChip);
    }
    
    public void MovePlayerFromCard(Color playerColor) {
        foreach (Transform chip in playersPlace) {
            if (chip.GetComponent<Chip>().ColorChip.color == playerColor) {
                Destroy(chip.gameObject);
                break;
            }
        }
    }

    public void RemoveAllPlayersFromCard() {
        foreach (Transform chip in playersPlace) {
            Destroy(chip.gameObject);
        }
    }
}
