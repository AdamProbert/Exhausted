using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private float horizontalInput;
    private float verticalInput;

    private float steeringAngle;
    
    [SerializeField] private WheelCollider f_r_wheel, f_l_wheel, r_r_wheel, r_l_wheel;
    [SerializeField] private Transform f_r_transform, f_l_transform, r_r_transform, r_l_transform;
    [SerializeField] private float maxSteerAngle;
    [SerializeField] private float motorForce;
    [SerializeField] private Rigidbody carRigidBody;
    [SerializeField] private Vector3 carCenterOfMass;
    [SerializeField] private bool frontWheelDrive;



    public void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
    }

    private void Steer()
    {
        steeringAngle = maxSteerAngle * horizontalInput;
        f_r_wheel.steerAngle = steeringAngle;
        f_l_wheel.steerAngle = steeringAngle;
    }

    private void Accelerate()
    {
        if(frontWheelDrive)
        {
            f_r_wheel.motorTorque = verticalInput * motorForce;
            f_l_wheel.motorTorque = verticalInput * motorForce;
        }
        else
        {
            r_r_wheel.motorTorque = verticalInput * motorForce;
            r_l_wheel.motorTorque = verticalInput * motorForce;
        }
        
    }

    private void UpdateAllWheelPoses()
    {
        UpdateWheelPose(f_r_wheel, f_r_transform);
        UpdateWheelPose(f_l_wheel, f_l_transform);
        UpdateWheelPose(r_r_wheel, r_r_transform);
        UpdateWheelPose(r_l_wheel, r_l_transform);
    }

    private void UpdateWheelPose(WheelCollider collider, Transform wheel)
    {
        Vector3 position = wheel.position;
        Quaternion rotation = wheel.rotation;

        collider.GetWorldPose(out position, out rotation);

        wheel.position = position;
        wheel.rotation = rotation;
    }

    private void Start() 
    {
        carRigidBody.centerOfMass = carCenterOfMass;  
    }

    private void FixedUpdate() 
    {
        GetInput();
        Steer();
        Accelerate();
        UpdateAllWheelPoses();    
    }
}
