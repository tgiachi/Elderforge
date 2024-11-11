using System.Collections;
using System.Collections.Generic;
using Data;
using TMPro;
using UnityEngine;

public class MessageLogScript : MonoBehaviour
{
    public TextMeshProUGUI messageLog;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("MessageLogScript Start");
        InstanceHolder.EventBusService.Subscribe(
            (LoggerEvent e) =>
            {
                messageLog.SetText(messageLog.text + "\n" + e.Message);
            }
        );
    }

    // Update is called once per frame
    void Update()
    {
    }
}
