using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityStandardAssets.Vehicles.Car;
using TMPro;

// [RequireComponent(typeof(CarController))]
[RequireComponent(typeof(WeaponController))]
[RequireComponent(typeof(Player))]

public class UserInput : MonoBehaviour
{

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

    private bool isActive = true;

    [SerializeField] bool debugMode;

    private void Awake() 
    {
        controls = new InputMaster();
        controls.Player.Enable();

        controls.Player.Zoom.performed += ctx => HandleZoom();

        // controls.Player.Weapon1.performed += ctx => HandleWeaponSwitch(1);
        // controls.Player.Weapon2.performed += ctx => HandleWeaponSwitch(2);
        // controls.Player.Weapon3.performed += ctx => HandleWeaponSwitch(3);
        // controls.Player.Weapon4.performed += ctx => HandleWeaponSwitch(4);
    }

    private void Start() 
    {
        carController = GetComponent<UnityStandardAssets.Vehicles.Car.CarController>();
        weaponController = GetComponent<WeaponController>();
        lockOnController = transform.GetComponentInChildren<LockOnController>();

        if(lockOnController)
        {
            controls.Player.LockOn.started += ctx => HandleLockOn(0);
            controls.Player.LockOn.performed += ctx => HandleLockOn(1);
            controls.Player.LockOn.canceled += ctx => HandleLockOn(2);
        }
    }

    private void Update() 
    {
        // Need this for Cinemachine free look camera
        CinemachineCore.GetInputAxis = GetAxisCustom;
    }

    // private void HandleWeaponSwitch(int weaponNumber)
    // {
    //     if(debugMode) Debug.Log("Player requested switch to weapon: " + weaponNumber);
    //     weaponController.SelectWeapon(weaponNumber);
    // }

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

    private void OnEnable() 
    {
        controls.Player.Enable();    
    }

    private void OnDisable() 
    {
        if(carController)carController.Move(0, 0, 1, 1, 0);
        controls.Player.Disable();
    }

    public void SetActive()
    {
        if(controls == null)
        {
            Awake();
        }
        controls.Player.Enable();
        isActive = true;
    }
    public void Deactivate()
    {
        controls.Player.Disable();
        isActive = false;
    }
}
