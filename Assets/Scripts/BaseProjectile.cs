using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BaseProjectile : MonoBehaviour
{
    private void OnCollisionEnter(Collision other) 
    {
        gameObject.SetActive(false);
    }
}
