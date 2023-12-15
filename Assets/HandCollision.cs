using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bhaptics.SDK2;
using System.Diagnostics;
using UnityEngine.Animations.Rigging;

public class HandCollision : MonoBehaviour
{
    [SerializeField] private GameObject rig;
    
    GameObject avatar;
    GameObject hands;
    GameObject spine;
    GameObject headset;
    GameObject RHFollow;
    GameObject LHFollow;
    GameObject RHLocation;
    GameObject LHLocation;
    Vector3 prevPosRH;
    Vector3 prevPosLH;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        avatar = GameObject.FindGameObjectWithTag("Avatar");
        hands = GameObject.FindGameObjectWithTag("Avatar Hands");
        spine = GameObject.FindGameObjectWithTag("Avatar Spine");
        headset = GameObject.FindGameObjectWithTag("MainCamera");
        RHFollow = GameObject.FindGameObjectWithTag("RH Follow");
        LHFollow = GameObject.FindGameObjectWithTag("LH Follow");
        RHLocation = GameObject.FindGameObjectWithTag("RH Location");
        LHLocation = GameObject.FindGameObjectWithTag("LH Location");
        animator = avatar.GetComponent<Animator>();
        prevPosRH = RHFollow.transform.position;
        prevPosLH = LHFollow.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the distance between the headset and the spine is less than 0.8f
        if (Vector3.Distance(headset.transform.position, spine.transform.position) < 0.8f)
        {
            // Set the rig to true and enable the RigBuilder
            rig.gameObject.SetActive(true);
            avatar.GetComponent<RigBuilder>().enabled = true;
            // Play the haptic feedback
            BhapticsLibrary.Play("vest");
            BhapticsLibrary.Play("left_hand");
            BhapticsLibrary.Play("right_hand");
            // Set the shaking animation to false
            animator.SetBool("Shaking", false);
            RHFollow.transform.position = Vector3.MoveTowards(RHFollow.transform.position, RHLocation.transform.position, 0.01f);
            LHFollow.transform.position = Vector3.MoveTowards(LHFollow.transform.position, LHLocation.transform.position, 0.01f);
        }
        // Check if the distance between the user hand and the avatar hand is less than 1.25f
        else if (Vector3.Distance(transform.position, hands.transform.position) < 2.0f)
        {
            RHFollow.transform.position = Vector3.MoveTowards(RHFollow.transform.position, prevPosRH, 0.01f);
            LHFollow.transform.position = Vector3.MoveTowards(LHFollow.transform.position, prevPosLH, 0.01f);
            // Stop the rig
            rig.gameObject.SetActive(false);
            // Disable the RigBuilder
            avatar.GetComponent<RigBuilder>().enabled = false;
            // Set the shaking animation to true
            animator.SetBool("Shaking", true);
            // Check if the distance between the user hand and the avatar hand is less than 0.25f
            if (Vector3.Distance(transform.position, hands.transform.position) < 0.25f)
            {
                // Play the haptic feedback
                BhapticsLibrary.Play("right_hand");
            }
        }
        // Set the rig to false and disable the RigBuilder
        else
        {
            RHFollow.transform.position = Vector3.MoveTowards(RHFollow.transform.position, prevPosRH, 0.01f);
            LHFollow.transform.position = Vector3.MoveTowards(LHFollow.transform.position, prevPosLH, 0.01f);
            // Set the shaking animation to false
            animator.SetBool("Shaking", false);
            // Stop the rig
            rig.gameObject.SetActive(false);
            // Disable the RigBuilder
            avatar.GetComponent<RigBuilder>().enabled = false;
        }
    }

    public void TestHaptic()
    {
        BhapticsLibrary.Play("left_hand");
        BhapticsLibrary.Play("right_hand");
        BhapticsLibrary.Play("vest");
        
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
