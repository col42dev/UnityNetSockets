using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class PersistentDataFileIO : MonoBehaviour {

	// Use this for initialization
	void Start () {


        string path = Application.persistentDataPath + "/persistent.txt";
        Debug.Log("persistentDataPath: " + path);
        StreamWriter fileWriter;


        if (File.Exists(path))
            fileWriter = File.AppendText(path);
        else
            fileWriter = File.CreateText(path);


        string datetime = DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss");
        fileWriter.WriteLine(datetime);
        fileWriter.Close();
        Debug.Log(File.ReadAllText(path));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
