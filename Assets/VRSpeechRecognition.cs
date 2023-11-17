using UnityEngine;
using System.Net.Sockets;
using System.IO;
using System;
using System.Text;
using UnityEngine.Windows;

public class VRSpeechRecognition : MonoBehaviour
{
    private TcpClient client;
    private NetworkStream stream;

    void Start()
    {

        Debug.Log("Hello, Unity!");
        // Get a list of available microphones
        string[] microphones = Microphone.devices;

        // Select a microphone (e.g., the first one in the list)
        string selectedMicrophone = microphones.Length > 0 ? microphones[0] : null;


        // Start recording from the selected microphone
        AudioClip audioClip = Microphone.Start(selectedMicrophone, true, 100, 44100);
        Microphone.End(selectedMicrophone);


        // Wait for the microphone to start recording
        while (Microphone.GetPosition(selectedMicrophone) <= 0) 
        {

        }

        Debug.Log("Found");

        // Convert audio data to a byte array
        float[] samples = new float[audioClip.samples * audioClip.channels];
        audioClip.GetData(samples, 0);
        byte[] audioData = new byte[samples.Length * 4];
        Buffer.BlockCopy(samples, 0, audioData, 0, audioData.Length);

        // Connect to the Python script
        client = new TcpClient("127.0.0.1", 12345);
        stream = client.GetStream();

        Debug.Log("Connected to python client");

        Debug.Log(audioData.Length);

        // Send the audio data to the Python script
        stream.Write(audioData, 0, audioData.Length);

        // Stop recording and close the connection
        Microphone.End(selectedMicrophone);
        stream.Close();
        client.Close();
    }

    void tts()
    {
        // Connect to the Python server
        TcpClient client = new TcpClient("localhost", 5555);
        Stream stream = client.GetStream();

        // Replace 'your_audio_source' with a reference to your AudioSource component
        AudioSource audioSource = GetComponent<AudioSource>();

        // Receive and play the audio data
        byte[] buffer = new byte[4096];
        using (MemoryStream ms = new MemoryStream())
        {
            int bytesRead;
            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                ms.Write(buffer, 0, bytesRead);
            }

            audioSource.clip = ToAudioClip(ms.ToArray(), 0, ms.ToArray().Length, 0, 44100);
            audioSource.Play();
        }

        // Close the connection
        stream.Close();
        client.Close();
    }

    public AudioClip ToAudioClip(byte[] audioData, int offset, int length, int offsetSamples, int frequency)
    {
        if (length <= 0 || frequency <= 0)
        {
            Debug.LogError("Invalid parameters for creating AudioClip.");
            return null;
        }

        float[] data = new float[length / 2];
        int rescaleFactor = 32767; // to convert shorts to floats

        for (int i = 0; i < length / 2; i++)
        {
            short val = (short)(audioData[i * 2 + offset] | (audioData[i * 2 + 1 + offset] << 8));
            data[i] = val / (float)rescaleFactor;
        }

        AudioClip audioClip = AudioClip.Create("AudioClip", length / 2 - offsetSamples, 1, frequency, false);
        audioClip.SetData(data, offsetSamples);

        return audioClip;
    }
}
