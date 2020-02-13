using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldCarController : MonoBehaviour
{
    private float horizontalInput;
    private float verticalInput;
    private float steeringAngle;
    private Vector3 velocity;

    [SerializeField][Range(0,1)][Tooltip("Decimal multiplied by usual wheel collider friction attributes. Lower number = higher skid")]
     private float brakeSkidMultiplier;

    private bool braking;
    
    [SerializeField] private WheelCollider f_r_wheel, f_l_wheel, r_r_wheel, r_l_wheel;
    [SerializeField] private Transform f_r_transform, f_l_transform, r_r_transform, r_l_transform;
    [SerializeField] public float maxSteerAngle;
    [SerializeField] private float motorForce;
    [SerializeField] private float brakeTorque;
    [SerializeField] private Rigidbody carRigidBody;
    [SerializeField] private Vector3 carCenterOfMass;
    [SerializeField] private bool frontWheelDrive;


    public void Move(float h, float v)
    {
        Debug.Log("Car input horizontal: " + h + "       vertical: " + v);
        this.horizontalInput = h;
        this.verticalInput = v;
        Steer();    
        Accelerate(v);
        UpdateAllWheelPoses(); 
    }

    public void Steer()
    {
        steeringAngle = maxSteerAngle * horizontalInput;
        f_r_wheel.steerAngle = steeringAngle;
        f_l_wheel.steerAngle = steeringAngle;
    }

    public void Accelerate(float speed, bool autoBrake=true)
    {
        Debug.Log("speed: " + speed);
        velocity = transform.InverseTransformDirection(carRigidBody.velocity);
        
        if (velocity.z > 0 && speed < 0)
        {
            if(!braking & autoBrake)
            {
                // We're braking
                BrakeAllWheels(true);
            }
        }
        else
        {
            BrakeAllWheels(false);
            // We're moving
            if(frontWheelDrive)
            {
                f_r_wheel.motorTorque = speed * motorForce;
                f_l_wheel.motorTorque = speed * motorForce;
            }
            else
            {
                r_r_wheel.motorTorque = speed * motorForce;
                r_l_wheel.motorTorque = speed * motorForce;
            }    
        }        
    }

    public void BrakeAllWheels(bool brake)
    {
        if(brake & !braking)
        {
            Debug.Log("Braking");
            Brake(f_r_wheel);
            Brake(f_l_wheel);
            Brake(r_r_wheel);
            Brake(r_l_wheel);
            braking = true;
        }

        if(!brake & braking)
        {
            Debug.Log("Releaing brake");
            ReleaseBrake(f_r_wheel);
            ReleaseBrake(f_l_wheel);
            ReleaseBrake(r_r_wheel);
            ReleaseBrake(r_l_wheel);
            braking = false;
        }
    }

    private void Brake(WheelCollider wheel)
    {
        wheel.brakeTorque = brakeTorque;

        WheelFrictionCurve forwardFriction = wheel.forwardFriction;
        forwardFriction.extremumSlip *= brakeSkidMultiplier;
        forwardFriction.extremumValue *= brakeSkidMultiplier;
        forwardFriction.asymptoteSlip *= brakeSkidMultiplier;
        forwardFriction.asymptoteValue *= brakeSkidMultiplier;

        wheel.forwardFriction = forwardFriction;

        WheelFrictionCurve sidewaysFriction = wheel.sidewaysFriction;
        sidewaysFriction.extremumSlip *= brakeSkidMultiplier;
        sidewaysFriction.extremumValue *= brakeSkidMultiplier;
        sidewaysFriction.asymptoteSlip *= brakeSkidMultiplier;
        sidewaysFriction.asymptoteValue *= brakeSkidMultiplier;
        wheel.sidewaysFriction = sidewaysFriction;
    }

    private void ReleaseBrake(WheelCollider wheel)
    {
        wheel.brakeTorque = 0;

        WheelFrictionCurve forwardFriction = wheel.forwardFriction;
        forwardFriction.extremumSlip /= brakeSkidMultiplier;
        forwardFriction.extremumValue /= brakeSkidMultiplier;
        forwardFriction.asymptoteSlip /= brakeSkidMultiplier;
        forwardFriction.asymptoteValue /= brakeSkidMultiplier;
        wheel.forwardFriction = forwardFriction;

        WheelFrictionCurve sidewaysFriction = wheel.sidewaysFriction;
        sidewaysFriction.extremumSlip /= brakeSkidMultiplier;
        sidewaysFriction.extremumValue /= brakeSkidMultiplier;
        sidewaysFriction.asymptoteSlip /= brakeSkidMultiplier;
        sidewaysFriction.asymptoteValue /= brakeSkidMultiplier;
        wheel.sidewaysFriction = sidewaysFriction;
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
}
