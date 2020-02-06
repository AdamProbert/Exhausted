using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CarController))]

public class UserInput : MonoBehaviour
{

    private float horizontalInput;
    private float verticalInput;

    private CarController carController;


    private void Start() 
    {
        carController = GetComponent<CarController>();    
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
