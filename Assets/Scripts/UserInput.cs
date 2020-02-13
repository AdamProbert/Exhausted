using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CarController))]
[RequireComponent(typeof(WeaponController))]

public class UserInput : MonoBehaviour
{

    public InputMaster controls; // Our custom InputSystem control scheme
    private Vector2 lookDelta;
    private Vector2 moveInput;

    private float firing = 0; // Float not bool cause InputSystem
    private bool zoomToggle = false;

    private UnityStandardAssets.Vehicles.Car.CarController carController;    // Reference to actual car controller we are controlling
    private WeaponController weaponController;


    private void Awake() 
    {
        controls = new InputMaster();

        // One event for pressing fire, second for releasing
        // controls.Player.Fire.performed += ctx => HandleFire(ctx.ReadValue<float>());
        controls.Player.Zoom.performed += ctx => HandleZoom();

        // controls.Player.Move.performed += ctx => HandleMove(ctx.ReadValue<Vector2>()); // This is read every frame so we can get 0 when no input..
        // controls.Player.Look.performed += ctx => HandleLook(ctx.ReadValue<Vector2>()); // This is read every frame so we can get 0 when no input..
        
    }

    private void Start() 
    {
        carController = GetComponent<UnityStandardAssets.Vehicles.Car.CarController>();
        weaponController = GetComponent<WeaponController>();
    }

    private void Update() 
    {
        // Need this for Cinemachine free look camera
        CinemachineCore.GetInputAxis = GetAxisCustom;
    }

    private void HandleFire(float fire)
    {   
        Debug.Log("Handle fire called with: " + fire);
        if(fire > 0)
        {
            weaponController.Fire();
        }
        else
        {
            weaponController.StopFiring();
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
        moveInput = controls.Player.Move.ReadValue<Vector2>();
        carController.Move(moveInput.x, moveInput.y, moveInput.y, 0);
    
        // carController.Move(moveInput.x, moveInput.y);

        firing = controls.Player.Fire.ReadValue<float>();
        HandleFire(firing);
    }


    private void OnEnable() 
    {
        controls.Player.Enable();    
    }

    private void OnDisable() 
    {
        controls.Disable();
    }
}
