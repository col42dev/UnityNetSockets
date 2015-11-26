using UnityEngine;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using SimpleJSON;

public class ServerSocket : MonoBehaviour
{

	const int
		kPort = 42210,
		kHostConnectionBacklog = 10;

    #region Private  Members
    private Socket mSocket;
    private IPAddress ip;
    static private ServerSocket mInstance;
    private List<Socket> mClients = new List<Socket>();
    System.IAsyncResult OnClientConnectResult = null;
    private SocketDebug mSocketDebug;
    JSONArray mBatchedContent = new JSONArray();
    #endregion

    #region Private Properties
    private IPAddress IP
    {
        get { if (ip == null) { ip = IPAddress.Parse(UnityEngine.Network.player.ipAddress); } return ip; }
    }
    public static ServerSocket Instance
    {
        get { return mInstance; }
    }
    #endregion

    #region Public Methods


    public void Update()
    {
        if (OnClientConnectResult != null)
        {
            mSocketDebug.DebugLog("lServer: Handling client connecting");
            try
            {
                Socket client = mSocket.EndAccept(OnClientConnectResult);

                mSocketDebug.DebugLog("lServer: Client connected");
                mClients.Add(client);
                SocketRead.Begin(client, OnReceive, OnReceiveError);
                OnClientConnectResult = null;
            }
            catch (System.Exception e)
            {
                mSocketDebug.DebugLogError("lServer: Exception when accepting incoming connection: " + e);
            }

            try
            {
                mSocket.BeginAccept(new System.AsyncCallback(OnClientConnect), mSocket);
            }
            catch (System.Exception e)
            {
                mSocketDebug.DebugLogError("lServer: Exception when starting new accept process: " + e);
            }

        }

        if ( mBatchedContent.Count > 0)
        {
            foreach (JSONData transaction in mBatchedContent)
            {
                Debug.Log(transaction.ToString());
            }

            while (mBatchedContent.Count > 0)
            {
                mBatchedContent.Remove(0);
            }
        }
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
        if (Host(kPort))
        {
            mSocketDebug.DebugLog("lServer: started");
        }
    }

	private bool Host (int port)
	{
        mSocketDebug.DebugLog("lServer: Hosting on " + IP + " : " + port);

        mSocket = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        mSocket.ReceiveBufferSize = 8192;

        try
		{
            mSocket.Bind (new IPEndPoint (IP, port));
            mSocket.Listen (kHostConnectionBacklog);
            mSocket.BeginAccept (new System.AsyncCallback (OnClientConnect), mSocket);
		}
		catch (System.Exception e)
		{
            mSocketDebug.DebugLogError("lServer: Exception when attempting to host (" + IP + " : " + port + "): " + e);
            mSocket = null;
			return false;
		}

		return true;
	}

    private void OnReceive(SocketRead read, byte[] data)
    {
        //string message = "lServer: Client " + mClients.IndexOf(read.Socket) + " says: " + Encoding.ASCII.GetString(data, 0, data.Length);
        //mSocketDebug.DebugLog(message);

        string json = Encoding.ASCII.GetString(data, 0, data.Length);

        JSONNode jsonNode = JSON.Parse(json);

        mBatchedContent = jsonNode.AsArray;

     }

    private void OnReceiveError(SocketRead read, System.Exception exception)
    {
        mSocketDebug.DebugLogError("lServer: Receive error: " + exception);
    }

    private void OnClientConnect (System.IAsyncResult result)
	{
        OnClientConnectResult = result;
	}

	private void OnEndHostComplete (System.IAsyncResult result)
	{
        mSocket = null;
	}
    #endregion






}
