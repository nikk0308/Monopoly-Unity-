using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chip : MonoBehaviour {
    [SerializeField] private Image chipColor;
    
    public Image ColorChip => chipColor;
    
    public void ColorChange(Color colorToChange) {
        chipColor.color = colorToChange;
    }
}
