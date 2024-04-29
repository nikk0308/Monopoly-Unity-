using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptOpen : MonoBehaviour
{
    [Header("1 - open, 2 - close")]
    public bool openOrClose;
    public GameObject objectUI;

    public void OpenOrCloseObject() {
        objectUI.gameObject.SetActive(openOrClose);
    }
}
