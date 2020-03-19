using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseDamager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] protected float damage;

    public float GetDamage()
    {
        return damage;
    }
}
