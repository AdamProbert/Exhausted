using UnityEngine;
using System.Collections;

// Car control interface
[RequireComponent(typeof(Car))]
public class CarControl : MonoBehaviour
{
    protected Car car;

    void Awake()
    {
        car = GetComponent<Car>();
    }

    protected void ControlCar(float acel, float turn)
    {
        car.Control = new Vector2(turn, acel);
    }
}
