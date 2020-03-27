using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;

public class CarSpecOverride : MonoBehaviour
{
    [SerializeField] public float vehicleMass = 1000f;
    [SerializeField] public Vector3 centrOfMass = new Vector3(0, -1, 0);
    [SerializeField] public float maxSteeringAngle = 30f;
    [SerializeField] public float maxSpeed = 80f;
    [SerializeField] public CarDriveType driveType = CarDriveType.FrontWheelDrive;
    [SerializeField] public float torqueOverAllWheels = 5000;

    [SerializeField] public float flyingRotationTorque = 10000;

    private void Start() 
    {
        GetComponentInParent<PlayerEventManager>().OnVehicleLoad();    
    }
}
