using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OculusSampleFramework;

public class VRCharacterMimic : MonoBehaviour
{
    public Transform playerLeftHand;
    public Transform playerRightHand;
    public Transform characterLeftHand;
    public Transform characterRightHand;
    public float rotationSpeed = 5f;

    private void Update()
    {
        UpdateCharacterHands();
    }


    void UpdateCharacterHands()
    {
        // Update left hand position and rotation
        characterLeftHand.position = playerLeftHand.position;
        characterLeftHand.rotation = playerLeftHand.rotation;

        // Update right hand position and rotation
        characterRightHand.position = playerRightHand.position;
        characterRightHand.rotation = playerRightHand.rotation;
    }
}
