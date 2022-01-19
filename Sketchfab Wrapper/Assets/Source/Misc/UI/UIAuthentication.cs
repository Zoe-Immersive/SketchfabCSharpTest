using System;
using UnityEngine;
using UnityEngine.UI;

public class UIAuthentication : MonoBehaviour
{
    public Text TxtTitle;
    public InputField InEmail;
    public InputField InPassword;

    public Button BtnLogin;
    public Button BtnLogout;

    private string m_TitleString;

    private void Awake()
    {
        // Listen for login or logout btn clicks
        BtnLogin.onClick.AddListener(Login);
        BtnLogout.onClick.AddListener(Logout);
        
        // Used to keep the origin title after logout
        m_TitleString = TxtTitle.text;
        SetElementsVisibility(false);
    }

    private void OnEnable()
    {
        TestApi.OnLogin += OnLogin;
        TestApi.OnLogout += OnLogout;
        TestApi.OnUserInformation += OnUserInformation;
    }

    private void OnDisable()
    {
        TestApi.OnLogin -= OnLogin;
        TestApi.OnLogout -= OnLogout;
        TestApi.OnUserInformation -= OnUserInformation;
    }

    public void Login()
    {
        TestApi.Login(InEmail.text, InPassword.text);
    }

    private void OnLogin()
    {
        SetElementsVisibility(true);
    }

    public void Logout()
    {
        TestApi.Logout();
    }

    private void SetElementsVisibility(bool _authenticated)
    {
        BtnLogin.gameObject.SetActive(!_authenticated);
        InEmail.gameObject.SetActive(!_authenticated);
        InPassword.gameObject.SetActive(!_authenticated);
        BtnLogout.gameObject.SetActive(_authenticated);
        if (!_authenticated)
        {
            TxtTitle.text = m_TitleString;
        }
    }

    private void OnLogout()
    {
        SetElementsVisibility(false);
    }

    private void OnUserInformation(SketchfabUserInfo _info)
    {
        TxtTitle.text = "Welcome "  + _info.Username;
    }
}
