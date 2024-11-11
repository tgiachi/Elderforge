using System.Collections;
using System.Collections.Generic;
using Elderforge.Network.Client.Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConnectToServerScript : MonoBehaviour
{
    public TextMeshProUGUI serverInput;

    public TextMeshProUGUI portInput;


    public Button connectButton;

    public void ConnectToServer()
    {
        InstanceHolder.NetworkClient = new NetworkClient(
            serverInput.text,
            5000,
            InstanceHolder.MessageTypes
        );

        InstanceHolder.NetworkClient.Connect();
    }

    // Start is called before the first frame update
    void Start()
    {
        serverInput.SetText("127.0.0.1");
        portInput.SetText("5000");

        connectButton.onClick.AddListener(OnClick);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnClick()
    {
        ConnectToServer();
    }
}
