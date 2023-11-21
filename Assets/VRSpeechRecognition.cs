using System;
using UnityEngine;
using System.Diagnostics;


public class VRSpeechRecognition : MonoBehaviour
{
    void Start()
    {
        // Path to your Python script
        string pythonScriptPath = "C:\\Users\\Spher\\Downloads\\sockets.py";

        // Create a process to run the Python script
        Process process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "python",
                Arguments = $"{pythonScriptPath}",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            }
        };

        // Start the process and capture its output
        process.Start();
        string output = process.StandardOutput.ReadToEnd();
        process.WaitForExit();

        // Do something with the output (e.g., update Unity UI)
        UnityEngine.Debug.Log(output);
    }
}