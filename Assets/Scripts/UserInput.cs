using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityStandardAssets.Vehicles.Car;
using TMPro;

// [RequireComponent(typeof(CarController))]
[RequireComponent(typeof(WeaponController))]
[RequireComponent(typeof(Player))]
[RequireComponent(typeof(PlayerEventManager))]

public class UserInput : MonoBehaviour
{
    private PlayerEventManager playerEventManager;
    public InputMaster controls; // Our custom InputSystem control scheme
    private Vector2 lookDelta;
    private Vector2 moveInput;

    private float accelerate;
    
    private float boost;
    private float brake;

    private float firing = 0; // Float not bool cause InputSystem
    private bool zoomToggle = false;

    private CarController carController;    // Reference to actual car controller we are controlling
    private WeaponController weaponController;
    private LockOnController lockOnController;

    private bool isActive = false;

    [SerializeField] bool debugMode;

    private void Awake() 
    {
        playerEventManager = GetComponent<PlayerEventManager>();
        controls = new InputMaster();
        playerEventManager.OnPlayerStateChanged += HandlePlayerStateChange;    
    }

    private void Start() 
    {
        Debug.Log("UserInput: Start called");
        carController = GetComponent<UnityStandardAssets.Vehicles.Car.CarController>();
        weaponController = GetComponent<WeaponController>();
        lockOnController = transform.GetComponentInChildren<LockOnController>();
    }

    private void Update() 
    {
        // Need this for Cinemachine free look camera
        CinemachineCore.GetInputAxis = GetAxisCustom;
    }

    private void HandleLockOn(float lockstate)
    {
        if(debugMode){Debug.Log("Handle lock called with" + lockstate);}

        switch (lockstate)
        {
            case 0:
                lockOnController.InitateLockOn();
                break;
            case 1:
                lockOnController.LockOn();
                break;
            case 2:
                lockOnController.RemoveLock();
                break;
        }
    }

    private void HandleZoom()
    {
        zoomToggle = !zoomToggle;
    }

    public float GetAxisCustom(string axisName)
    {
        //-------------------------------------------------------------------------------------------------//
        // Unfortunately this function is required as Cinemachine does not support the new input system yet..
        //-------------------------------------------------------------------------------------------------//
        if(controls.Player.enabled)
        {
            lookDelta = controls.Player.Look.ReadValue<Vector2>(); // reads the available camera values and uses them.
            lookDelta.Normalize();

            if (axisName == "Look X")
            {
                return lookDelta.x;
            }
            else if (axisName == "Zoom")
            {
                if(zoomToggle)
                {
                    return 100;
                }
                else
                {
                    return -100;
                }
            }   
        }

        return 0;
    }

    private void FixedUpdate() 
    {
        if(isActive)
        {
            moveInput = controls.Player.Turn.ReadValue<Vector2>();
            accelerate = controls.Player.Accelerate.ReadValue<float>();
            boost = controls.Player.Boost.ReadValue<float>();
            brake = controls.Player.Brake.ReadValue<float>();
            brake *= -1;
            carController.Move(moveInput.x, accelerate, brake, 0, boost);
        
            // firing = controls.Player.Fire.ReadValue<float>();
            // HandleFire(firing);
        }
    }

    public void HandlePlayerStateChange(Player.state newstate)
    {
        Debug.Log("UserInput: Player state changed to:" + newstate);
        if(newstate != Player.state.Alive)
        {
            Deactivate();
        }
        else
        {
            Activate();
        }
    }

    public void Activate()
    {       
        controls.Player.Enable();
        isActive = true;
        controls.Player.Zoom.performed += ctx => HandleZoom();
        
        if(lockOnController)
        {
            controls.Player.LockOn.started += ctx => HandleLockOn(0);
            controls.Player.LockOn.performed += ctx => HandleLockOn(1);
            controls.Player.LockOn.canceled += ctx => HandleLockOn(2);
        }
    }

    public void Deactivate()
    {
        controls.Player.Disable();
        isActive = false;
    }
}
