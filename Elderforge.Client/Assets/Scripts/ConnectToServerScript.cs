using System;
using System.Collections;
using System.Collections.Generic;
using Elderforge.Network.Client.Services;
using Elderforge.Network.Packets;
using Elderforge.Network.Packets.System;
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


        try
        {
            InstanceHolder.NetworkClient = new NetworkClient(
           "127.0.0.1",
           5000,
           InstanceHolder.MessageTypes
       );

            Debug.Log("Connecting to server");
            InstanceHolder.NetworkClient.Connect();

        }
        catch (Exception e)
        {
            Debug.Log("Error connecting to server: " + e.Message);
        }

        Debug.Log("Connected to server");

        var message = InstanceHolder.NetworkClient.SubscribeToMessage<VersionMessage>().Subscribe(
            (VersionMessage e) =>
            {
                Debug.Log("VersionMessage: " + e.Version);
            }
        );




    }

    // Start is called before the first frame update
    void Start()
    {

        Debug.Log("ConnectToServerScript Start");

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
