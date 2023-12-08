using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCollision : MonoBehaviour
{
    [SerializeField] private GameObject rig;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the colliding GameObject has the tag "Avatar"
        if (other.CompareTag("Avatar"))
        {
            // Perform actions when the collision with an "Avatar" occurs
            // Debug.Log("Collision with Avatar detected!");
            rig.gameObject.SetActive(true);
            // You can add more actions or functions here as needed
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the colliding GameObject has the tag "Avatar"
        if (other.CompareTag("Avatar"))
        {
            // Perform actions when the collision with an "Avatar" ends
            // Debug.Log("Collision with Avatar exited!");
            rig.gameObject.SetActive(false);
            // You can add more actions or functions here as needed
        }
    }
}
