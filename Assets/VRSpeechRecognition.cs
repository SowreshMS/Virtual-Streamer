using UnityEngine;
using System.Diagnostics;
using System.Text;
using System.Collections;

public class VRSpeechRecognition : MonoBehaviour
{
    private Process pythonProcess;

    void Start()
    {
        UnityEngine.Debug.Log("hello");
        StartCoroutine(ActivatePythonScriptAfterDelay());
    }

    IEnumerator ActivatePythonScriptAfterDelay()
    {
        yield return new WaitForSeconds(2f);

        StartPythonScript();
    }

    void StartPythonScript()
    {
        UnityEngine.Debug.Log("Python time");

        string pythonScriptPath = @"C:/Users/Spher/Downloads/sockets.py";

        pythonProcess = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "python",
                Arguments = $"{pythonScriptPath}",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            },
            EnableRaisingEvents = true // Allow capturing process exit event
        };

        // Event handler for capturing the process exit event
        pythonProcess.Exited += (sender, e) =>
        {
            string output = pythonProcess.StandardOutput.ReadToEnd();
            string errorOutput = pythonProcess.StandardError.ReadToEnd();

            if (!string.IsNullOrEmpty(output))
            {
                UnityEngine.Debug.Log("Python script output: " + output);
            }

            if (!string.IsNullOrEmpty(errorOutput))
            {
                UnityEngine.Debug.LogError("Python script error: " + errorOutput);
            }

            pythonProcess.Dispose(); // Dispose of the process to free resources
        };

        UnityEngine.Debug.Log("Python time part2");
        pythonProcess.Start();
    }

    // Optionally, you may want to stop the Python process when the Unity application quits
    private void OnApplicationQuit()
    {
        if (pythonProcess != null && !pythonProcess.HasExited)
        {
            pythonProcess.Kill();
            pythonProcess.Dispose();
        }
    }
}
