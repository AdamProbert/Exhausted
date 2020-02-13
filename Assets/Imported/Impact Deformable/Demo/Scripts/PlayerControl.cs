using UnityEngine;
using System.Collections;

// Player input on car control
public class PlayerControl : CarControl
{
    void Update()
    {
        ControlCar(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"));
    }
}
