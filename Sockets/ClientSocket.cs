using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using SimpleJSON;

public class ClientSocket : MonoBehaviour
{
	const int kPort = 42210;

	#region Private  Members
	private Socket mSocket;
	private IPAddress ip;
    static private ClientSocket mInstance;
    private SocketDebug mSocketDebug;

    JSONArray mBatchedContent = new JSONArray();
    const float kBatchTransferPollPeriodSecs = 4f;
    #endregion

    #region Private Properties
    private IPAddress IP
	{
		get { if (ip == null) { ip =  IPAddress.Parse (UnityEngine.Network.player.ipAddress); } return ip; }
	}

    public static ClientSocket Instance
    {
        get { return mInstance;}
    }
    #endregion

    #region Public Methods
    public void Update()
    {
        if (!mSocket.Connected)
        {
            mSocketDebug.DebugLog("lClient: Connecting...");

            try
            {
                mSocket.Connect(new IPEndPoint(IP, kPort));
            }
            catch (System.Exception e)
            {
                mSocketDebug.DebugLogError("lClient: Exception: " + e.Message);
                mSocketDebug.DebugLogError("lClient: Failed to connect to " + ip + " on port " + kPort);
            }

            if (mSocket.Connected)
            {
                mSocketDebug.DebugLog("lClient: Successfully connected to " + ip + " on port " + kPort);
                mSocketDebug.DebugLog("lClient: started");
                SocketRead.Begin(mSocket, OnReceive, OnError);
            }
        }
    }

    public void POST(JSONData content)
    {
        // mBatchedContent.Add(mBatchedContent.Count.ToString(), content);  - disabled while this crash callstack investigated. https://ptowndev.slack.com/files/rajantande/F0C7CBYA3/screen_shot_2015-10-09_at_16.20.55.png
    }
    #endregion

    #region Private Methods
    private void Awake()
    {
        mInstance = this;
    }

    private void Start()
    {
        mSocketDebug = GetComponent<SocketDebug>();
        mSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        mSocket.SendBufferSize = 8192;

        //StartCoroutine( BatchPOST()); - disabled while this crash callstack investigated. https://ptowndev.slack.com/files/rajantande/F0C7CBYA3/screen_shot_2015-10-09_at_16.20.55.png
    }

    private IEnumerator BatchPOST()
    {
        while (true)
        {
            if (mBatchedContent.Count > 0)
            {
                string packet = mBatchedContent.ToString();
                byte[] data = Encoding.ASCII.GetBytes(packet);
                Debug.Log("BatchPOST Length = " + data.Length);
                mSocket.Send(data);

                while (mBatchedContent.Count > 0)
                {
                    mBatchedContent.Remove(0);
                }

                //mSocketDebug.DebugLog("lClient: client said: " + packet);
            }
            yield return new WaitForSeconds(kBatchTransferPollPeriodSecs);
        }
    }

    private void OnReceive(SocketRead read, byte[] data)
    {
        mSocketDebug.DebugLog("lClient: OnReceive");
        mSocketDebug.DebugLog(Encoding.ASCII.GetString(data, 0, data.Length));
    }

    private void OnError(SocketRead read, System.Exception exception)
    {
        mSocketDebug.DebugLogError("lClient: Receive error: " + exception);
    }

    #endregion






}
