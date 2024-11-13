using System;
using System.Collections;
using System.Collections.Generic;
using Elderforge.Network.Client.Services;
using Elderforge.Network.Packets.System;
using Serilog;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using ILogger = UnityEngine.ILogger;

public class LoginHandlerScript : MonoBehaviour
{
    // Start is called before the first frame update

    public Button loginButton;

    public TMP_InputField serverInputField;

    public TMP_InputField portInputField;

    public TextMeshProUGUI versionText;


    void Start()
    {
        loginButton.onClick.AddListener(OnLoginClick);

        serverInputField.text = "127.0.0.1";

        portInputField.text = "5000";
    }

    private void OnLoginClick()
    {
        Log.Logger.Information("Connecting to {Host}:{Port}", serverInputField.text, portInputField.text);

        ElderforgeInstanceHolder.NetworkClient.SubscribeToMessage<VersionMessage>()
            .Subscribe(
                message =>
                {
                    versionText.text = "v" + message.Version;
                }
            );

        ElderforgeInstanceHolder.NetworkClient.SubscribeToMessage<ServerReadyMessage>()
            .Subscribe(
                message =>
                {
                    SceneManager.LoadScene("WorldScene");
                }
            );

        ElderforgeInstanceHolder.NetworkClient.Connect(serverInputField.text, int.Parse(portInputField.text));

    }

    // Update is called once per frame
    void Update()
    {
        if (ElderforgeInstanceHolder.NetworkClient.IsConnected)
        {
            ElderforgeInstanceHolder.NetworkClient.PoolEvents();
        }
    }
}
