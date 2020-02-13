using UnityEngine;
using System.Collections;

// Camera script for CarDerby demoscene
public class View : MonoBehaviour 
{
    public Transform Target;

    void Update()
    {
        // Follow target
        transform.LookAt(Target, Vector3.up);
    }
}
