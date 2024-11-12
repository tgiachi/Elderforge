using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginHandlerScript : MonoBehaviour
{
    // Start is called before the first frame update


    public Button loginButton;

    public TMP_InputField serverInputField;

    public TMP_InputField portInputField;


    void Start()
    {
        loginButton.onClick.AddListener(OnLoginClick);

        serverInputField.text = "127.0.0.1";

        portInputField.text = "5000";
    }

    private void OnLoginClick()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
