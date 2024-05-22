using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Position : MonoBehaviour
{
    public int arrayIndex;
    public int cellIndex;

    public Position(int arrayIndex = 0, int cellIndex = 0) {
        this.arrayIndex = arrayIndex;
        this.cellIndex = cellIndex;
    }
}
