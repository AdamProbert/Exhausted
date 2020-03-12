using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoConnectChainJoint : MonoBehaviour
{
    void Awake()
    {
        GetComponent<CharacterJoint>().connectedBody = transform.parent.GetComponent<Rigidbody>();
    }
}