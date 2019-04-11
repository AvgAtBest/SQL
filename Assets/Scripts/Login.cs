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
    //login input fields
    public InputField usernameLogin;
    public InputField passwordLogin;
    [Header("RegisterDeets")]
    //register input fields
    public InputField usernameCreate;
    public InputField passwordCreate;
    public InputField emailCreate;
    public InputField passwordConfirm;
    //Warning Tooltips
    public GameObject invalidEmailAddressText;
    public GameObject wrongPasswordConfirmText;
    public GameObject incorrectSecCode;
    [Header("Recovery")]
    public InputField emailRecovery;
    private static System.Random random = new System.Random();
    //Sec code
    public string code;
    public InputField codeInput;
    public GameObject emptyFieldWarning;
    [Header("Strings")]
    //strings used for data
    public string inputUsername;
    public string inputPassword;
    public string inputPasswordConfirm;
    public string inputEmail;
    public string newPasswordString;
    public string confirmNewPassString;
    [Header("Port")]
    //server port
    public int port = 587; // PORTS TO TRY IF ONE DOESNT WORK: 25, 587, 465
    [Header("Scripts")]
    //UIManager script
    public UIManager UICeo;
    [Header("Password Reset")]
    //Input Fields for Password Reset
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
        //creates a new form to insert user to database
        WWWForm insertUserForm = new WWWForm();

        //add each respective field into the database, using the username, password and email via their string
        //Calls post references in php
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
        //Obtain the text from each of the input fields for later (to add to database)
        inputUsername = usernameCreate.text;
        inputPassword = passwordCreate.text;
        inputPasswordConfirm = passwordConfirm.text;
        inputEmail = emailCreate.text;


        //Checks if the text string in the input Password field matches the text string in the password confirm field
        //And the email text string contains the @ symbol
        if (inputPassword == inputPasswordConfirm && emailCreate.text.Contains("@"))
        {
            //Start Coroutine Create User using the strings for username, password and email gained from text.
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
        //obtains string from the text fields
        inputUsername = usernameLogin.text;
        inputPassword = passwordLogin.text;
        //grabs the input username string and input password string and uses those string for the LoginUser Coroutine
        StartCoroutine(LoginUser(inputUsername, inputPassword));
        Debug.Log("Login");

    }
    //Inserts LoginUser data in database
    IEnumerator LoginUser(string _username, string _password)
    {
        //Finds the LoginUser.php file located in htDocs xampp
        //Creates a new form to input data
        string loginUserURL = "http://localhost/SQueaLsystem/LoginUser.php";
        WWWForm loginUserForm = new WWWForm();
        //checks each field in the database, using the username, password and email via their string
        //Calls post references in php
        loginUserForm.AddField("usernamePost", _username);
        loginUserForm.AddField("passwordPost", _password);

        WWW www = new WWW(loginUserURL, loginUserForm);
        //returns data from form
        yield return www;

        Debug.Log(www.text);

        //if the data is found and matches
        if (www.text == "Login success")
        {
            //load scene
            //login success
            LogIntoScene(1);
        }

    }
    public void LogIntoScene(int sceneIndex)
    {
        //load into the scene
        SceneManager.LoadScene(sceneIndex);
    }
    //Checks to see if their is a email in the database that matches
    IEnumerator CheckUser(string _email)
    {
        //obtains the CheckUser.php from database
        string checkUserURL = "http://localhost/SQueaLsystem/CheckUser.php";
        //creates new form
        WWWForm checkUserForm = new WWWForm();

        //obtains the email that has been put in
        checkUserForm.AddField("emailPost", _email);

        //sends data to post
        WWW www = new WWW(checkUserURL, checkUserForm);
        //returns data
        yield return www;

        //if the form finds a matching email
        if(www.text == "user found")
        {
            Debug.Log("Sent email");
            //sends a email to the address
            SendEmail(_email);
            //toggles the panel on to input security code
            UICeo.SwitchToSecCodePanel();
            //turns off tooltip
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
        //creates a string with 8 random characters/numbers
        code = RandomString(8);
        //creates a new message
        MailMessage mail = new MailMessage();
        //mail is going to be sent from sqlunityclasssydney@gmail.com email with "MrJerkenburger's Jerken Burgers" as senders name
        MailAddress ourMail = new MailAddress("sqlunityclasssydney@gmail.com", "MrJerkenburger's Jerken Burgers");

        //adds the user email as recipient
        mail.To.Add(email);
        //references the mail to send from
        mail.From = ourMail;
        //Header name
        mail.Subject = "SQueaL Games User";
        mail.Body = "Hello user\n Here is the recovery code you requested to reset your password: " + code; // sends body message + the security code
        //sends email from gmail, checks port and credential for validity
        SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
        smtpServer.Port = port; // PORTS TO TRY IF ONE DOESNT WORK: 25, 587, 465
        smtpServer.Credentials = new System.Net.NetworkCredential("sqlunityclasssydney@gmail.com", "sqlpassword") as ICredentialsByHost;

        //Establishes a encrypted link
        smtpServer.EnableSsl = true;
        //Gmail checks to see if the certificate that has been sent is valid
        ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate cert, X509Chain chain, SslPolicyErrors policyErrors)
        { return true; };
        //gmail sends the mail
        smtpServer.Send(mail);

        Debug.Log("Success");


    }
    public static string RandomString(int length)
    {
        //Characters and numbers below used for the security code
        const string chars = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz0123456789";
        //creates a string containing 8 of the characters/numbers above in a random order
        return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());

    }
    public void ForgotPass()
    {
        //if there is actually text in the inputfield and the input fields contains the @ symbol
        if (emailRecovery.text != "" && emailRecovery.text.Contains("@"))
        {
            //grabs the email that was put into the inputfield
            inputEmail = emailRecovery.text;
            Debug.Log(inputEmail);
            //sends a reset password email to the email address that has been input
            StartCoroutine(CheckUser(inputEmail));

        }
        else
        {
            //wont send a email, and toggles on a tooltip warning user of a invalid address
            emptyFieldWarning.SetActive(true);
            Debug.Log("The input field is empty");
        }

    }
    public void PasswordReset()
    {
        //if the code input into the text matches the security code
        if (codeInput.text == code)
        {
            incorrectSecCode.SetActive(false);
            //switches panel
            UICeo.SwitchToPasswordResetPanel();
            //ChangePass();
            Debug.Log("Password Reset");
        }
        else
        {
            incorrectSecCode.SetActive(true);
            Debug.Log("The security code is incorrect");
        }
    }
    #endregion
    #region Change Password
    public void ChangePass()
    {
        //Obtains the text from the input field as a string
        newPasswordString = newPasswordField.text;
        confirmNewPassString = confirmNewPassword.text;
        //if both strings match
        if (newPasswordString == confirmNewPassString)
        {
            //change the password in the database
            StartCoroutine(ChangePassword(newPasswordString, inputEmail));
        }
    }
    IEnumerator ChangePassword(string _password, string _email)
    {
        Debug.Log(_password + " " + _email);
        //finds the UpdatePassword.php in the database
        string changePasswordURL = "http://localhost/SQueaLsystem/UpdatePassword.php";
        //sends a changePassword form
        WWWForm changePasswordForm = new WWWForm();
        //sends the string to change to the php
        changePasswordForm.AddField("passwordPost", _password);
        changePasswordForm.AddField("emailPost", _email);
        //changes the users password
        WWW www = new WWW(changePasswordURL, changePasswordForm);
        //returns the data
        yield return www;

  
    }
    #endregion
   
}
