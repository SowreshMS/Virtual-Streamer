using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandFollow : MonoBehaviour
{
    [SerializeField] private GameObject userRightHand;
    [SerializeField] private GameObject userLeftHand;
    [SerializeField] private GameObject avatarFollowRightHand;
    [SerializeField] private GameObject avatarFollowLeftHand;
    private Vector3 prevPosRH;
    private Vector3 prevPosLH;
    private Vector3 currPosRH;
    private Vector3 currPosLH;
    private Vector3 changeRH;
    private Vector3 changeLH;
    // Start is called before the first frame update
    void Start()
    {
        prevPosRH = userRightHand.transform.position;
        prevPosLH = userLeftHand.transform.position;
        currPosRH = userRightHand.transform.position;
        currPosLH = userLeftHand.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        currPosRH = userRightHand.transform.position;
        currPosLH = userLeftHand.transform.position;
        changeRH = currPosRH - prevPosRH;
        changeLH = currPosLH - prevPosLH;
        avatarFollowRightHand.transform.position += changeRH;
        avatarFollowLeftHand.transform.position += changeLH;
        prevPosRH = currPosRH;
        prevPosLH = currPosLH;
    }
}
