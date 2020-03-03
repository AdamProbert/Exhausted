using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shrapnel : MonoBehaviour
{
    [SerializeField] float damage = 1;

    private void OnCollisionEnter(Collision other) 
    {
        if(other.gameObject.layer == 8 && other.gameObject.GetComponent<Player>())
        {
            other.gameObject.GetComponent<Player>().TakeDamage(damage);
        }
    }
}
