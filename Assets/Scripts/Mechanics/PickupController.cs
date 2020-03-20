using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupController : MonoBehaviour
{
    PlayerEventManager playerEventManager;

    [SerializeField] LayerMask pickupLayer;

    private void Awake() 
    {
        playerEventManager = GetComponent<PlayerEventManager>();
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(pickupLayer == (pickupLayer | (1 << other.gameObject.layer)))
        {
            Pickup pickup = other.gameObject.GetComponentInParent<Pickup>();
            pickup.PickItUp();
            playerEventManager.OnPickupItem(pickup);
        }
    }
}
