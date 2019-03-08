using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Buttons")]
    public Button confirmButton;
    public Button cancelButton;
    public Button loginMainMenuButton;
    public Button registerMainMenuButton;
    public Button recoveryMainMenuButton;
    public Button sendSecCodeButton;
    public Button confirmSecCodeButton;
    [Header("Panels")]
    public GameObject loginPanel;
    public GameObject registerPanel;
    public GameObject recoveryPanel;
    public GameObject secCodePanel;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
