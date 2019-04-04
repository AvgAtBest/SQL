using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Buttons
    [Header("Buttons")]
    //List of Toggleable buttons
    public Button confirmButton;
    public Button cancelButton;
    public Button loginMainMenuButton;
    public Button registerMainMenuButton;
    public Button recoveryMainMenuButton;
    public Button sendSecCodeButton;
    public Button confirmSecCodeButton;
    #endregion
    #region Panels
    [Header("Panels")]
    //List of Gameobject panels
    public GameObject loginPanel;
    public GameObject registerPanel;
    public GameObject recoveryPanel;
    public GameObject secCodePanel;
    public GameObject codeAcceptedPanel;
    #endregion
    #region Toggle
    //Toggle the password reset panel on
    public void SwitchToPasswordResetPanel()
    {
        //Code Accepted panel on
        codeAcceptedPanel.SetActive(true);
        //Security code panel off
        secCodePanel.SetActive(false);
    }
    //Toggle the Security Code input panel on
    public void SwitchToSecCodePanel()
    {
        //security panel on
        secCodePanel.SetActive(true);
        //recovery panel off
        recoveryPanel.SetActive(false);

    }
    #endregion
}
