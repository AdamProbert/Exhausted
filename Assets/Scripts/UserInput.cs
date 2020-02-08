using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CarController))]
[RequireComponent(typeof(WeaponController))]

public class UserInput : MonoBehaviour
{

    private float horizontalInput;
    private float verticalInput;

    private CarController carController;
    private WeaponController weaponController;


    private void Start() 
    {
        carController = GetComponent<CarController>();    
        weaponController = GetComponent<WeaponController>();
    }

    public void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        GetInput();
        carController.Move(horizontalInput, verticalInput);
    }
}
