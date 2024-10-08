using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ConsoleToUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI debugText;
    private string logMessages = "";

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        if (type == LogType.Error || type == LogType.Exception)
        {
            logMessages += $"<color=red>{logString}</color>\n";
            logMessages += $"<color=red>{stackTrace}</color>\n";
        }
        else if (type == LogType.Log)
        {
            logMessages += logString + "\n";
        }
        debugText.text = logMessages;
    }

}
