using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupController : MonoBehaviour
{
    [SerializeField] LayerMask pickupLayer;

    private void OnTriggerEnter(Collider other) 
    {
        if(pickupLayer == (pickupLayer | (1 << other.gameObject.layer)))
        {
            Pickup pickup = other.gameObject.GetComponentInParent<Pickup>();
            pickup.PickItUp();
            if(pickup.PType == Pickup.PTypes.Armour)
            {
                BroadcastMessage("MessagePickupArmour");
            }
        }
    }
}
