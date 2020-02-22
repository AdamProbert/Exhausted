using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NodeCanvas.StateMachines;
using UnityEngine.UI;

[RequireComponent(typeof(MeshRenderer))]

public class DestroyCar : MonoBehaviour
{
    [SerializeField] Material destroyMaterial;
    [SerializeField] float burnSpeed;
    [SerializeField] ParticleSystem carExplodePS;
    [SerializeField] float explosionForce;
    [SerializeField] private GameObject[] disableComponents;
    [SerializeField] private GameObject[] enableComponents;


    private Material instantiatedBurnMaterial;
    private IEnumerator playerBurnRoutine;
    private MeshRenderer m_Renderer;


    private void Awake()
    {
        playerBurnRoutine = BurnAfterDeath();
        m_Renderer = GetComponent<MeshRenderer>();
    }

    // Broadcast receiver    
    public void PlayerDied()
    {
        EnableDestroyedParts();
        StartCoroutine(playerBurnRoutine);
        Explosion();
    }

    private void EnableDestroyedParts()
    {
        foreach(GameObject g in disableComponents)
        {
            g.SetActive(false);
        }

        foreach(GameObject g in enableComponents)
        {
            g.transform.parent = null;
            g.SetActive(true);
        }
    }

    private void Explosion()
    {
        if(explosionForce > 0f)
        {
            transform.root.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, 1f, 2f, ForceMode.VelocityChange);
        }
        
        Instantiate(carExplodePS, transform.position, Quaternion.identity);
    }
    
    private IEnumerator BurnAfterDeath() 
    {
        instantiatedBurnMaterial = new Material(destroyMaterial); 
        m_Renderer.material = instantiatedBurnMaterial;
        float burnDissolveAmountMax = 1.1f;
        float currentDissolveAmount = 0f;
        float currentBurnAmount = 0f;
        Debug.Log("routine running: burn speed: " + burnSpeed);
        while (currentDissolveAmount <= burnDissolveAmountMax)
        {
            instantiatedBurnMaterial.SetFloat("_Burn", currentDissolveAmount += burnSpeed);    
            currentDissolveAmount += burnSpeed;

            if(currentDissolveAmount > burnDissolveAmountMax /2)
            {
                instantiatedBurnMaterial.SetFloat("_Dissolve", currentBurnAmount += burnSpeed);    
                currentBurnAmount += burnSpeed;
            }
            
            yield return new WaitForSeconds(burnSpeed*5);
        }

        while (currentBurnAmount <= burnDissolveAmountMax)
        {
            instantiatedBurnMaterial.SetFloat("_Dissolve", currentBurnAmount += burnSpeed);
            currentBurnAmount += burnSpeed;
            yield return new WaitForSeconds(burnSpeed*5);
        }
    }
}
