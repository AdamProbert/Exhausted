using UnityEngine;
using System.Collections;

public class MetalRain : MonoBehaviour 
{
	// Rain prefab
	public GameObject RainObject;

	// Pointer to deformable component in the cube
	ImpactDeformable impactDeformable;

	void Start () 
	{
		// Start rain procedures
		StartCoroutine(Rain());
		
		// Capture deformable component
		impactDeformable = GetComponent<ImpactDeformable>();	
	}
	
	IEnumerator Rain () 
	{
		while (true)
		{
			// Create random drops
			GameObject.Instantiate(RainObject, new Vector3((Random.value - 0.5f) * 20, (Random.value * 40) + 10, (Random.value - 0.5f) * 20), Quaternion.identity);
			yield return new WaitForSeconds(Random.value * 0.2f + 0.1f);
		}
	}

    // Repair 25% of damage on click
    void OnMouseDown()
	{
		impactDeformable.Repair(0.25f);
	}

    // Set floor hardness
    public void SetHardness(float value)
    {
        impactDeformable.Hardness = value;
    }

    // Toggle floor mesh collider deformation
    public void SetDeformMeshCollider(bool fd)
    {
        impactDeformable.DeformMeshCollider = fd;
    }
}
