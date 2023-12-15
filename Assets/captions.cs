using UnityEngine;
using System;
using System.IO;
using TMPro;

public class Captions : MonoBehaviour
{
    public Transform targetObject;
    private TextMeshPro textMesh;

    void Start()
    {
        // Find the TextMeshPro object by tag
        GameObject textObject = GameObject.FindGameObjectWithTag("Text");

        // Check if the text object exists
        if (textObject != null)
        {
            // Get the TextMeshPro component
            textMesh = textObject.GetComponent<TextMeshPro>();
        }
        else
        {
            Debug.LogError("Text object with tag 'Text' not found in the scene.");
        }

        // Set initial position
        MoveTextToTargetPosition();
    }

    void Update()
    {
        try 
        {
            // Update text
            if (textMesh != null)
            {
                textMesh.text = File.ReadAllText(Path.Combine(Application.dataPath, "speaker.txt"));
            }
            //textMesh.text = "How are you doing today.";
        }
        catch (Exception ex) {
            Debug.Log("An error occurred while reading the file: " + ex.Message);
        }

    }

    void MoveTextToTargetPosition()
    {
        // Check if the headset is active (in VR mode)
        bool isVRActive = OVRManager.isHmdPresent;

        
        // If VR is not active, move the text to the target position
        if (targetObject != null)
        {
            textMesh.transform.position = targetObject.transform.position;
        }
        
    }
}
