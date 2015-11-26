using UnityEngine;
using UnityEditor;
using System.Collections;
using SimpleJSON;
using System.Timers;


/// <summary>
/// Client Socket Unit Test
/// </summary>
public class ClientSocketUT   {

    private  Timer mTimer;
    private int  mDuration;

    public  void TestStartPOST()
    {
        Debug.Log("Started TestStartPOST");
        mTimer = new System.Timers.Timer();
        mTimer.Elapsed += new ElapsedEventHandler(ClientBatchPOST);
        mTimer.Interval = 100;
        mTimer.Enabled = true;
        mDuration = 0;
    }

    private  void  ClientBatchPOST(object sender, ElapsedEventArgs e)
    {
        JSONData packet = new JSONData("SamplePacket:" + mDuration);
        ClientSocket.Instance.POST(packet);


        Timer timer = (Timer)sender;

        mDuration += (int)timer.Interval;

        if (mDuration > 1000)
        {
            timer.Stop();
            Debug.Log("Completed TestStartPOST");
        }
    }
}
