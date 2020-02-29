using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TesterScript : MonoBehaviour
{
    [SerializeField] GameObject car;
    [SerializeField] BaseWeapon weapon;
    
    [SerializeField] Vector3 spawnpoint;

    [SerializeField] Explode barrel;

    bool weaponfiriing = false;
    // Update is called once per frame

    void Update()
    {
        if(Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            barrel.DOIT();
            
            // car.BroadcastMessage("PlayerDied");
        }

        // if(Keyboard.current.backspaceKey.wasPressedThisFrame)
        // {
        //     Destroy(car);
        //     car = Instantiate(car, spawnpoint, Quaternion.identity);
        // }

    }
}
