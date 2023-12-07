using UnityEngine;
using System.IO;

public class captions : MonoBehaviour
{
    public Transform targetObject; // Assign the target object in the Inspector
    public Canvas canvasToMove; // Assign the canvas to move in the Inspector

    void Start()
    {
        if (targetObject == null || canvasToMove == null)
        {
            Debug.LogError("Please assign the target object and canvas to move in the Inspector.");
            return;
        }

        // Load text from "speaker.txt" and display it on the canvas (assuming TextMeshPro)
        // TextAsset textAsset = Resources.Load<TextAsset>("speaker");
       
        canvasToMove.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = File.ReadAllText(Path.Combine(Application.dataPath, "speaker.txt"));
        

        MoveCanvasToTargetPosition();
    }

    void MoveCanvasToTargetPosition()
    {
        // Get the target position from the target object
        Vector3 targetPosition = targetObject.position;

        // Convert the target position to screen space
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(targetPosition);

        // Set the canvas position to the screen position
        canvasToMove.transform.position = screenPosition;
    }

    void update()
    {
        canvasToMove.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = File.ReadAllText(Path.Combine(Application.dataPath, "speaker.txt"));
    }
}
