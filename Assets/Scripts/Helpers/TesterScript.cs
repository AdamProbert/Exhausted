using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TesterScript : MonoBehaviour
{
    [SerializeField] GameObject car;
    [SerializeField] BaseWeapon weapon;
    
    [SerializeField] Vector3 spawnpoint;

    [SerializeField] Transform instantiationPoint;

    [SerializeField] InteractableMapProp barrel;

    private InteractableMapProp myBarrel;

    bool weaponfiriing = false;
    // Update is called once per frame

    
    // private void Start() 
    // {
    //     weapon.Init();     
    // }
    
    void Update()
    {
        if(Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            myBarrel.ExplodeProp();
            // weapon.StartFiring();
            
            // car.BroadcastMessage("PlayerDied");
        }

        if(Keyboard.current.backspaceKey.wasPressedThisFrame)
        {
            if(myBarrel != null)
            {
                Destroy(myBarrel);
            }
            myBarrel = Instantiate(barrel, instantiationPoint.position, instantiationPoint.rotation).GetComponent<InteractableMapProp>();
        }

    }
}
