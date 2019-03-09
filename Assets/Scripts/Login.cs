#region Unity Systems
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
#endregion
#region System Security directories
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Linq;
#endregion

public class Login : MonoBehaviour
{
    #region Variables
    [Header("LoginDeets")]
    public InputField usernameLogin;
    public InputField passwordLogin;
    [Header("RegisterDeets")]
    public InputField usernameCreate;
    public InputField passwordCreate;
    public InputField emailCreate;
    public InputField passwordConfirm;
    public GameObject invalidEmailAddressText;
    public GameObject wrongPasswordConfirmText;
    [Header("Recovery")]
    public InputField emailRecovery;
    private static System.Random random = new System.Random();
    public string code;
    public InputField codeInput;
    public GameObject emptyFieldWarning;
    [Header("Strings")]
    public string inputUsername;
    public string inputPassword;
    public string inputPasswordConfirm;
    public string inputEmail;
    [Header("Port")]
    public int port = 587; // PORTS TO TRY IF ONE DOESNT WORK: 25, 587, 465
    [Header("Scripts")]
    public UIManager UICeo;
    #endregion
    private void Awake()
    {
        UICeo = GameObject.Find("UIManager").GetComponent<UIManager>();

    }
    #region Register Data
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



        if (inputPassword == inputPasswordConfirm && emailCreate.text.Contains("@"))
        {
            StartCoroutine(CreateUser(inputUsername, inputPassword, inputEmail));
            Debug.Log("testing");
            if(inputPassword == inputPasswordConfirm)
            {
                wrongPasswordConfirmText.SetActive(false);
            }
            if (emailCreate.text.Contains("@"))
            {
                invalidEmailAddressText.SetActive(false);
            }

        }
        else if(inputPassword != inputPasswordConfirm || emailCreate.text.Contains(""))
        {

            invalidEmailAddressText.SetActive(!emailCreate.text.Contains("@"));

            wrongPasswordConfirmText.SetActive(inputPassword != inputPasswordConfirm);
        }

    }
    #endregion
    #region LoginData
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
    #endregion
    #region Recovery Data
    public void SendEmail(string email)
    {
        code = RandomString(8);

        MailMessage mail = new MailMessage();
        MailAddress ourMail = new MailAddress("sqlunityclasssydney@gmail.com", "MrJerkenburger's Jerken Burgers");

        mail.To.Add(email);
        mail.From = ourMail;

        mail.Subject = "SQueaL Games User";
        mail.Body = "Hello" + inputUsername + "\n Here is the recovery code you requested to reset your password: " + code;

        SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
        smtpServer.Port = port; // PORTS TO TRY IF ONE DOESNT WORK: 25, 587, 465
        smtpServer.Credentials = new System.Net.NetworkCredential("sqlunityclasssydney@gmail.com", "sqlpassword") as ICredentialsByHost;

        smtpServer.EnableSsl = true;

        ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate cert, X509Chain chain, SslPolicyErrors policyErrors)
        { return true; };

        smtpServer.Send(mail);
        //if(codeInput.text == code)
        //{
        //    UICeo.SwitchPanel();
        //}
        Debug.Log("Success");


    }
    public static string RandomString(int length)
    {
        const string chars = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz0123456789";
        return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());

    }
    public void ForgotPass()
    {
        if (emailRecovery.text != "" && emailRecovery.text.Contains("@"))
        {
            inputEmail = emailRecovery.text;
            SendEmail(inputEmail);
            UICeo.SwitchToSecCodePanel();
            emptyFieldWarning.SetActive(false);

        }
        else
        {
            emptyFieldWarning.SetActive(true);
            Debug.Log("The input field is empty");
        }

    }
    public void PasswordReset()
    {

        if (codeInput.text == code)
        {
            UICeo.SwitchToPasswordResetPanel();
            Debug.Log("Password Reset");
        }
        else
        {
            Debug.Log("The security code is incorrect");
        }
    }
    #endregion

}
