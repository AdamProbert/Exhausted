using UnityEngine;
using System.Collections;

// Front and rear bumper on cars
public class CarBumper : MonoBehaviour 
{
    void Awake()
    {
        GetComponent<ImpactDeformable>().OnDeform += CarBumper_OnDeform;
    }

    public void OnDisable()
    {
        GetComponent<ImpactDeformable>().OnDeform -= CarBumper_OnDeform;
    }    

    // Impact Deformable OnDeform event
    void CarBumper_OnDeform(ImpactDeformable deformable)
    {
        // Check damage level
        if (deformable.StructuralDamage > 0.1f)
        {
            // Detach from car
            transform.parent = transform.parent.parent;
            Rigidbody body = gameObject.AddComponent<Rigidbody>();
            body.mass = 0.1f;

            // Destroy bumper in 10 seconds
            Destroy(this);
            Destroy(gameObject, 10);
        }
    }
}
