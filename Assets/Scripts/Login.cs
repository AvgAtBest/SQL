#region Unity Systems
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
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
    public string newPasswordString;
    public string confirmNewPassString;
    [Header("Port")]
    public int port = 587; // PORTS TO TRY IF ONE DOESNT WORK: 25, 587, 465
    [Header("Scripts")]
    public UIManager UICeo;
    [Header("Password Reset")]
    public InputField newPasswordField;
    public InputField confirmNewPassword;

    #endregion
    #region Awake
    private void Awake()
    {
        //Find GameManager game object in scene
        UICeo = GameObject.Find("UIManager").GetComponent<UIManager>();

    }
    #endregion
    #region Register Data
    //Creates user and inputs them into database, referencing _username string, _password string and _email string
    IEnumerator CreateUser(string _username, string _password, string _email)
    {
        //Finds the InsertUser.php file located in htDocs xampp
        //grabs info from database
        string createUserURL = "http://localhost/SQueaLsystem/InsertUser.php";

        WWWForm insertUserForm = new WWWForm();
        insertUserForm.AddField("usernamePost", _username);
        insertUserForm.AddField("passwordPost", _password);
        insertUserForm.AddField("emailPost", _email);

        WWW www = new WWW(createUserURL, insertUserForm);
        //Return the form data, adds new data to user table 
        yield return www;

        Debug.Log(www.text);
    }

    public void InputRegisterData()
    {
        //sets the "Create username" input field to true
        usernameCreate.enabled = true;
        //sets the "password create" input field to true
        passwordCreate.enabled = true;
        //sets the "email create" input field to true
        emailCreate.enabled = true;
        inputUsername = usernameCreate.text;
        inputPassword = passwordCreate.text;
        inputPasswordConfirm = passwordConfirm.text;
        inputEmail = emailCreate.text;


        //Checks if the text string in the input Password field matches the text string in the password confirm field
        //And the email text string contains the @ symbol
        if (inputPassword == inputPasswordConfirm && emailCreate.text.Contains("@"))
        {
            //Start Coroutine Create User using the text in username, password and email.
            StartCoroutine(CreateUser(inputUsername, inputPassword, inputEmail));
            Debug.Log("testing");
            //if the password matches
            if (inputPassword == inputPasswordConfirm)
            {
                //disable "unmatching password" tooltip
                wrongPasswordConfirmText.SetActive(false);
            }
            //if the emailField text contains the @ symbol
            if (emailCreate.text.Contains("@"))
            {
                //disable "invalid email" tooltip
                invalidEmailAddressText.SetActive(false);
            }

        }
        //if the inputpassword does not match the confirm password text, or the email text is blank
        else if (inputPassword != inputPasswordConfirm || emailCreate.text.Contains(""))
        {
            //Enables "invalid email" tooltip
            invalidEmailAddressText.SetActive(!emailCreate.text.Contains("@"));
            //enables "unmatching password" tooltip
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

        if (www.text == "Login success")
        {
            LogIntoScene(1);
        }

    }
    public void LogIntoScene(int sceneIndex)
    {

        SceneManager.LoadScene(sceneIndex);
    }
    IEnumerator CheckUser(string _email)
    {
        string checkUserURL = "http://localhost/SQueaLsystem/CheckUser.php";
        WWWForm checkUserForm = new WWWForm();
        checkUserForm.AddField("emailPost", _email);

        WWW www = new WWW(checkUserURL, checkUserForm);
        yield return www;

        if(www.text == "user found")
        {
            Debug.Log("Sent email");
            SendEmail(_email);
            UICeo.SwitchToSecCodePanel();
            emptyFieldWarning.SetActive(false);
        }
        else
        {
            Debug.Log("No email found.");
        }

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
        mail.Body = "Hello user\n Here is the recovery code you requested to reset your password: " + code;

        SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
        smtpServer.Port = port; // PORTS TO TRY IF ONE DOESNT WORK: 25, 587, 465
        smtpServer.Credentials = new System.Net.NetworkCredential("sqlunityclasssydney@gmail.com", "sqlpassword") as ICredentialsByHost;

        smtpServer.EnableSsl = true;

        ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate cert, X509Chain chain, SslPolicyErrors policyErrors)
        { return true; };

        smtpServer.Send(mail);

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
            Debug.Log(inputEmail);
            StartCoroutine(CheckUser(inputEmail));
            //UICeo.SwitchToSecCodePanel();
            //emptyFieldWarning.SetActive(false);

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
            //ChangePass();
            Debug.Log("Password Reset");
        }
        else
        {
            Debug.Log("The security code is incorrect");
        }
    }
    #endregion
    #region Change Password
    public void ChangePass()
    {
        //
        newPasswordString = newPasswordField.text;
        confirmNewPassString = confirmNewPassword.text;

        if (newPasswordString == confirmNewPassString)
        {
            StartCoroutine(ChangePassword(newPasswordString, inputEmail));
        }
    }
    IEnumerator ChangePassword(string _password, string _email)
    {
        Debug.Log(_password + " " + _email);
        string changePasswordURL = "http://localhost/SQueaLsystem/UpdatePassword.php";
        WWWForm changePasswordForm = new WWWForm();
        changePasswordForm.AddField("passwordPost", _password);
        changePasswordForm.AddField("emailPost", _email);

        WWW www = new WWW(changePasswordURL, changePasswordForm);
        yield return www;

  
    }
    #endregion
   
}
