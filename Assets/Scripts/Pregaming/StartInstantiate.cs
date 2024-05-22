using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartInstantiate : MonoBehaviour {
    [SerializeField] private GameObject MainMenu;

    void Start() {
        foreach (Transform child in transform) {
            child.gameObject.SetActive(true);
        }
        StartCoroutine(DeactivateWindows());
    }

    IEnumerator DeactivateWindows() {
        yield return null;

        foreach (Transform child in transform) {
            child.gameObject.SetActive(false);
        }
        MainMenu.gameObject.SetActive(true);
    }
}
