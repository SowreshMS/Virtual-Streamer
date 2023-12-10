using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bhaptics.SDK2;
using System.Diagnostics;

public class HandCollision : MonoBehaviour
{
    [SerializeField] private GameObject rig;
    GameObject avatar;
    GameObject hands;
    // Start is called before the first frame update
    void Start()
    {
        avatar = GameObject.FindGameObjectWithTag("Avatar");
        hands = GameObject.FindGameObjectWithTag("Avatar Hands");
    }

    // Update is called once per frame
    void Update()
    {
        // Check for collision with the avatar
        if (Vector3.Distance(transform.position, avatar.transform.position) < 2.0f)
        {
            rig.gameObject.SetActive(true);
        }
        else
        {
            //rig.gameObject.SetActive(false);
        }
        if (Vector3.Distance(transform.position, hands.transform.position) < 1.0f)
        {
            TestHaptic();
        }
        else
        {
        }
    }

    public void TestHaptic()
    {
        BhapticsLibrary.Play("left_hand");
        BhapticsLibrary.Play("right_hand");

        /*BhapticsLibrary.PlayParam("left_hand",
            intensity: 1f,  // The value multiplied by the original value
            duration: 2f,   // The value multiplied by the original value
            angleX: 20f,    // The value that rotates around global Vector3.up(0~360f)
            offsetY: 0.3f   // The value to move up and down(-0.5~0.5)
            );

        BhapticsLibrary.PlayParam("right_hand",
            intensity: 1f,  // The value multiplied by the original value
            duration: 2f,   // The value multiplied by the original value
            angleX: 20f,    // The value that rotates around global Vector3.up(0~360f)
            offsetY: 0.3f   // The value to move up and down(-0.5~0.5)
            ); */

        //BhapticsLibrary.StopAll();
    }
    private void OnTriggerEnter(Collider other)
    {
        // Check if the colliding GameObject has the tag "Avatar"
        if (other.CompareTag("Avatar"))
        {
            // Perform actions when the collision with an "Avatar" occurs
            // Debug.Log("Collision with Avatar detected!");
            // rig.gameObject.SetActive(true);
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
            // rig.gameObject.SetActive(false);
            // You can add more actions or functions here as needed
        }
    }
}
