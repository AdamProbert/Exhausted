using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TesterScript : MonoBehaviour
{
    [SerializeField] GameObject car;
    // Update is called once per frame
    void Update()
    {
        if(Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            BroadcastMessage("PlayerDied");
        }

        if(Keyboard.current.backspaceKey.wasPressedThisFrame)
        {
            Destroy(car);
            car = Instantiate(car, transform.position, Quaternion.identity, transform);
        }

    }
}
