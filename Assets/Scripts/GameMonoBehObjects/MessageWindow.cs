using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MessageWindow : MonoBehaviour
{
    [SerializeField] private TMP_Text messageText;
    [SerializeField] private Button closeError;
    
    public static MessageWindow Instance { get; private set; }     
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
    void Start()
    {
        closeError.onClick.AddListener(CloseError);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) {
            CloseError();
        }
    }
    
    public void ShowMessage(string message) {
        messageText.text = message;
        gameObject.SetActive(true);
    }
        
    private void CloseError() {
        gameObject.SetActive(false);
    }
}
