using UnityEngine;
using UnityEditor;
using System.Collections;

/// <summary>
/// Server Test Suite UI
/// </summary>
public class ServerTestUI  {

    [MenuItem("Test/Server/ClientPOST")]
    private static void StartClientPOST()
    {
        ClientSocketUT clientSocketUT = new ClientSocketUT();
        clientSocketUT.TestStartPOST();

    }
}
