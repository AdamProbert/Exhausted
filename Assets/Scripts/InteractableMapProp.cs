using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Explode))]

public class InteractableMapProp : MonoBehaviour
{

    [SerializeField] LayerMask collisionLayers;
    [SerializeField] int collisionBeforeDeath;
    private int currentCollisionCount = 0;

    private Explode explode;

    // Start is called before the first frame update
    void Start()
    {
        explode = GetComponent<Explode>();
    }

    private void OnCollisionEnter(Collision other) 
    {
        if(other.gameObject.layer == collisionLayers)
        {
            currentCollisionCount += 1;
        }
        if(currentCollisionCount >= collisionBeforeDeath)
        {
            explode.DOIT();
            Destroy(this.gameObject, 5f);
        }    
    }
}
