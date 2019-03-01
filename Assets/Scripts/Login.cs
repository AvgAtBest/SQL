using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Login : MonoBehaviour
{
    [Header("LoginDeets")]
    public InputField usernameLogin;
    public InputField passwordLogin;
    [Header("RegisterDeets")]
    public InputField usernameCreate;
    public InputField passwordCreate;
    public InputField emailCreate;
    public InputField passwordConfirm;
    [Header("Recover")]
    public InputField emailRecovery;
    [Header("Buttons")]
    public Button confirmButton;
    public Button cancelButton;
    public Button loginButton;
    public Button registerButton;
    public Button recoveryButton;
    [Header("Strings")]
    public string inputUsername;
    public string inputPassword;
    public string inputPasswordConfirm;
    public string inputEmail;
    [Header("Panels")]
    public GameObject loginPanel, registerPanel, recoveryPanel;
    // Use this for initialization

    public void Update()
    {
        //usernameCreate.text = inputPassword;
        //passwordCreate.text = inputPassword;
        //emailCreate.text = inputEmail;



    }
    IEnumerator CreateUser(string _username, string _password, string _email)
    {
        //grabs info from database
        string createUserURL = "http://localhost/SQueaLsystem/InsertUser.php";

        WWWForm insertUserForm = new WWWForm();
        insertUserForm.AddField("usernamePost", _username);
        insertUserForm.AddField("passwordPost", _password);
        insertUserForm.AddField("emailPost", _email);

        WWW www = new WWW(createUserURL, insertUserForm);
        yield return www;

        Debug.Log(www.text);
    }
    
    public void InputRegisterData()
    {

        usernameCreate.enabled = true;
        passwordCreate.enabled = true;
        emailCreate.enabled = true;
        inputUsername = usernameCreate.text;
        inputPassword = passwordCreate.text;
        inputPasswordConfirm = passwordConfirm.text;
        inputEmail = emailCreate.text;

        if (inputPassword == inputPasswordConfirm)
        {
            StartCoroutine(CreateUser(inputUsername, inputPassword, inputEmail));
            Debug.Log("testing");

        }

    }
    public void InputLoginData()
    {
        inputUsername = usernameLogin.text;
        inputPassword = passwordLogin.text;

        StartCoroutine(LoginUser(inputUsername, inputPassword));
        Debug.Log("Login");
    }
    IEnumerator LoginUser(string _username, string _password)
    {
        string loginUserURL = "http://localhost/SQueaLsystem/LoginUser.php";
        WWWForm loginUserForm = new WWWForm();
        loginUserForm.AddField("usernamePost", _username);
        loginUserForm.AddField("passwordPost", _password);

        WWW www = new WWW(loginUserURL, loginUserForm);

        yield return www;
        Debug.Log(www.text);
    }

}
