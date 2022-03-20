using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;

public class TLELoader : MonoBehaviour
{
    public UICommunicator communicator;
    public SimulationManager simulationManager;
    public string satCount;
    const string path = "Assets/TLEs/sample";
    const string ext = ".txt";
    Thread thread;
    float progressLevel = 0f;
    List<string> fileLines;
    int loadCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        fileLines = new List<string>();

        ThreadStart tStart = new ThreadStart(() => { LoadTLE(path + satCount + ext); });
        thread = new Thread(tStart);
        thread.Start();
        StartCoroutine("SyncLoadProgress");
        StartCoroutine("ApplyTLE");
    }

    void LoadTLE(string path)
    {
        FileInfo fileInfo;
        StreamReader reader;

        fileInfo = new FileInfo(path);
        reader = new StreamReader(fileInfo.OpenRead());

        long length = fileInfo.Length;
        long curPos = 0L;

        // Debug.Log("Start Reading");
        while (reader.Peek() >= 0)
        {
            string line = reader.ReadLine();
            fileLines.Add(line);
            curPos += line.Length;
            // Debug.Log(line);
            
            progressLevel = (float) curPos / length;

            // Debug.LogFormat("{0,4:f2}% reading...", progressLevel * 100f);
        }
        progressLevel = 1f;
        // Debug.Log("Read Complete!");
    }

    IEnumerator SyncLoadProgress()
    {
        while (thread.IsAlive)
        {
            communicator.ChangeTLELoadProgress(progressLevel);
            // Debug.Log(progressLevel);
            yield return new WaitForSeconds(0.1f);
        }
        communicator.ChangeTLELoadProgress(progressLevel);
        // Debug.Log(progressLevel);
    }

    IEnumerator ApplyTLE()
    {
        while (thread.IsAlive)
        {
            if (fileLines.Count != 0 && fileLines.Count > loadCount && fileLines.Count % 3 == 0)
            {
                for (int i=loadCount; i<fileLines.Count; i+=3)
                {
                    string  str1 = fileLines[i],
                            str2 = fileLines[i+1],
                            str3 = fileLines[i+2];

                    Debug.LogError(i);
                    Debug.Log(str1);
                    Debug.Log(str2);
                    Debug.Log(str3);
                    
                    simulationManager.AddSatellite(str1, str2, str3);
                    if (loadCount != 0) yield return new WaitForSeconds(1f);

                    loadCount += 3;
                }
            }
            yield return new WaitForSeconds(3f);
        }
        for (int i=loadCount; i<fileLines.Count; i+=3)
        {
            string  str1 = fileLines[i],
                    str2 = fileLines[i+1],
                    str3 = fileLines[i+2];

            simulationManager.AddSatellite(str1, str2, str3);
            yield return new WaitForSeconds(1f);

            loadCount += 3;
        }
        loadCount = 0;
    }
}
