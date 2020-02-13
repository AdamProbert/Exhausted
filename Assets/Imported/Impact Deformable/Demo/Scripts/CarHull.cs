using UnityEngine;
using System.Collections;

// Hull of a car
public class CarHull : MonoBehaviour 
{
    public AudioSource Audio;
    public ImpactDeformable[] Bumpers;

    void Awake()
    {
        GetComponent<ImpactDeformable>().OnDeformForce += CarHull_OnDeformForce;
        foreach (ImpactDeformable impactDeformable in Bumpers)
            impactDeformable.OnDeformForce += CarHull_OnDeformForce;
    }

    void OnDisable()
    {
        GetComponent<ImpactDeformable>().OnDeformForce -= CarHull_OnDeformForce;
        foreach (ImpactDeformable impactDeformable in Bumpers)
            impactDeformable.OnDeformForce += CarHull_OnDeformForce;
    }

    // Play crash sound when deformation event fired
    void CarHull_OnDeformForce(ImpactDeformable impactDeformable, Vector3 point, Vector3 force)
    {
        if (!Audio.isPlaying)
        {
            Audio.pitch = Random.Range(0.2f, 1f);
            Audio.Play();
        }        

        Audio.volume = Mathf.Max(force.magnitude * 5, Audio.volume);
    }

}
