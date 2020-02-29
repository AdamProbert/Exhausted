using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Explode))]
[RequireComponent(typeof(Rigidbody))]

public class InteractableMapProp : MonoBehaviour
{

    [SerializeField] LayerMask collisionLayers;
    [SerializeField] int collisionBeforeDeath;
    public int currentCollisionCount = 0;
    public bool exploded = false;

    private Explode explode;

    // Start is called before the first frame update
    void Start()
    {
        explode = GetComponent<Explode>();
    }

    public void ExplodeProp()
    {
        Debug.Log("Exploding");
        exploded = true;
        explode.DOIT();
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision other) 
    {
        if((collisionLayers & 1 << other.gameObject.layer) == 1 << other.gameObject.layer)
        {
            currentCollisionCount += 1;
        }

        if(currentCollisionCount >= collisionBeforeDeath)
        {
            if(!exploded)
            {
                ExplodeProp();
            }
        }    
    }
}

