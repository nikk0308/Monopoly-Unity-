using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Congratulations : MonoBehaviour {
    
    [SerializeField] private TMP_Text textCongratulations;
    [SerializeField] private Button finishGame;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject canvas;
    
    public static Congratulations Instance { get; private set; }     
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

    public void EndOfGame(string textToSet) {
        gameObject.SetActive(true);
        textCongratulations.text = textToSet;
    }
    
    private void Start()
    {
        finishGame.onClick.AddListener(CloseAll);
    }

    private void CloseAll() {
        foreach (Transform child in canvas.transform) {
            child.gameObject.SetActive(false);
        }
        mainMenu.gameObject.SetActive(true);
    }
}
