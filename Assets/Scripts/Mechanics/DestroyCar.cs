using System.Collections;
using UnityEngine;
using Cinemachine;
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(CinemachineImpulseSource))]

public class DestroyCar : MonoBehaviour
{
    PlayerEventManager playerEventManager;
    [SerializeField] Material destroyMaterial;
    [SerializeField] float burnSpeed;
    [SerializeField] float burnTickRate;
    [SerializeField] ParticleSystem carExplodePS;
    [SerializeField] ParticleSystem carExplodePS2;
    [SerializeField] float explosionForce;
    [SerializeField] private GameObject[] disableComponents;
    [SerializeField] private GameObject[] enableComponents;


    private CinemachineImpulseSource cinemachineImpulseSource;
    private Material instantiatedBurnMaterial;
    private IEnumerator playerBurnRoutine;
    private MeshRenderer m_Renderer;

    private void Awake()
    {
        playerEventManager =  transform.root.GetComponent<PlayerEventManager>();
        playerEventManager.OnPlayerStateChanged += HandlePlayerStateChange;
        playerBurnRoutine = BurnAfterDeath();
        m_Renderer = GetComponent<MeshRenderer>();
        cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
        GetComponent<ImpactDeformable>().OverrideMaster = true;
    }

    // Broadcast receiver    
    public void HandlePlayerStateChange(Player.state newstate)
    {
        if(newstate == Player.state.Dead)
        {
            EnableDestroyedParts();
            StartCoroutine(playerBurnRoutine);
        }
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
        
        ParticleSystem x = Instantiate(carExplodePS, transform.position, Quaternion.identity);
        Destroy(x.gameObject, 7f);
        cinemachineImpulseSource.GenerateImpulse();
    }

    private void Explosion2()
    {
        if(explosionForce > 0f)
        {
            Vector3 randsplodePosition = transform.position;
            randsplodePosition.x += Random.Range(-3, 3);
            randsplodePosition.y += Random.Range(-3, 3);
            randsplodePosition.z += Random.Range(-3, 3);

            transform.root.GetComponent<Rigidbody>().AddExplosionForce(explosionForce/2, randsplodePosition, 1f, 2f, ForceMode.VelocityChange);
        }
        
        ParticleSystem x = Instantiate(carExplodePS2, transform.position, Quaternion.identity);
        Destroy(x.gameObject, 7f);
        cinemachineImpulseSource.GenerateImpulse();
    }
    
    private IEnumerator BurnAfterDeath() 
    {
        Explosion();
        instantiatedBurnMaterial = new Material(destroyMaterial); 
        m_Renderer.material = instantiatedBurnMaterial;
        yield return new WaitForSeconds(Random.Range(0.3f, 1f));
        Explosion2();
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
            
            yield return new WaitForSeconds(burnTickRate);
        }

        while (currentBurnAmount <= burnDissolveAmountMax)
        {
            instantiatedBurnMaterial.SetFloat("_Dissolve", currentBurnAmount += burnSpeed);
            currentBurnAmount += burnSpeed;
            yield return new WaitForSeconds(burnTickRate);
        }

        Destroy(this.transform.root.gameObject);
    }
}
