using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NodeCanvas.StateMachines;
using UnityEngine.UI;

[RequireComponent(typeof(MeshFilter))]
public class DestroyMesh : MonoBehaviour
{
    [SerializeField] Material destroyMaterial;
    private Material instantiatedBurnMaterial;
    [SerializeField] float burnSpeed;
    private IEnumerator playerBurnRoutine;

    [SerializeField] MeshRenderer mainCarRenderer;


    private void Awake()
    {
        playerBurnRoutine = BurnAfterDeath();
        instantiatedBurnMaterial = new Material(destroyMaterial);      
    }

    public void DestroyIt()
    {
        StartCoroutine(playerBurnRoutine);
    }

    public void PlayerDied()
    {
        DestroyIt();
    }
    
    private IEnumerator BurnAfterDeath() 
    {
        mainCarRenderer.material = instantiatedBurnMaterial;
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
