using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Waypoint : MonoBehaviour
{
    public Waypoint previousWaypoint;
    public Waypoint nextWaypoint;
    [SerializeField] public bool start;
    [SerializeField] public bool finish;

    [SerializeField] private GameObject visuals;

    [Range(0f, 5f)]
    public float width = 1.5f;

    public void EnableVisuals()
    {
        visuals.SetActive(true);
    }

    public void DisableVisuals()
    {
        visuals.SetActive(false);
    }

    public Vector3 GetPosition()
    {
        Vector3 minBound = transform.position + transform.right * width /2f;
        Vector3 maxBound = transform.position - transform.right * width /2f;

        return Vector3.Lerp(minBound, maxBound, Random.Range(0,1f));
    }
}
