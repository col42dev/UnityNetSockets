using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Text;

public class SocketDebug : MonoBehaviour {

    #region Private  Members

    [SerializeField]
    private GameObject TextLog;

    [SerializeField]
    private string Filter;

    private int mDebugLogIndex = 0;
    private Queue<string> mDebugMessage = new Queue<string>();

    #endregion

    #region Public Methods
    // Use this for initialization
    private void Start () {
        Application.logMessageReceivedThreaded += HandleLog;
        TextLog.GetComponent<Text>().text = "";
    }

    public void Update () {
        while (mDebugMessage.Count > 0)
        {
            Debug.Log(mDebugMessage.Dequeue());
        }
    }

    public void DebugLog(string logString)
    {
        mDebugMessage.Enqueue(mDebugLogIndex + ")" + logString);
        mDebugLogIndex++;
    }

    public void DebugLogError(string logString)
    {
        mDebugMessage.Enqueue(mDebugLogIndex + ")" + logString);
        mDebugLogIndex++;
    }
    #endregion

    #region Private Methods
    private void HandleLog(string logString, string stackTrace, LogType type)
    {
        if (logString.Contains(Filter))
        {
            TextLog.GetComponent<Text>().text += "\n";
            if (TextLog.GetComponent<Text>().text.Length + logString.Length < 2046)
            {
                TextLog.GetComponent<Text>().text += logString;
            }
        }
    }
    #endregion

}
