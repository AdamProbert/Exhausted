using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BaseWeapon))]
public class AutoAimAndFIre : MonoBehaviour
{
    [SerializeField] private bool autoFindTarget;
    [SerializeField][Range(0,180)] protected float maxRotation;
    [SerializeField][Range(.1f, 10f)] protected float rotationSpeed;
    [SerializeField][Range(5f, 100f)] private float searchRadius;
    [SerializeField] private LayerMask searchLayers;
    [SerializeField] private LayerMask raycastIgnoreLayers;
    [SerializeField] private Vector3 aimOffset = new Vector3(0,1,-1);

    private Player target;
    private float turnRateRadians;

    BaseWeapon weapon;

    private void Start() 
    {
        weapon = GetComponent<BaseWeapon>();    
        turnRateRadians = rotationSpeed * Mathf.PI;
    }

    private void Update() 
    {
        if(weapon.currentStatus == BaseWeapon.status.Active)
        {
            CheckTargetAlive();

            if(autoFindTarget)
            {
                CheckCurrentTargetValid();
            }   
            
            if(target == null && autoFindTarget)
            {
                GetPlayerTarget();
            }
            
            TargetEnemy();
            
            AutoFire();
        }
        
    }
    

    void AutoFire()
    {
        if(target != null)
        {
            // Ray cast from weapon to target player, shoot if hit
            RaycastHit hit;
            // Does the ray hit player object
            if (Physics.Raycast(weapon.projectileSpawn.position, target.centrePoint.position - weapon.projectileSpawn.position, out hit, Mathf.Infinity, raycastIgnoreLayers))
            {
                Debug.DrawRay(weapon.projectileSpawn.position, (target.centrePoint.position - weapon.projectileSpawn.position), Color.yellow);
                RaycastHit returnhit;
                // Check if the return raycast works too
                if (!Physics.SphereCast(target.centrePoint.position, .5f, (weapon.projectileSpawn.position - target.centrePoint.position).normalized, out returnhit, hit.distance, raycastIgnoreLayers))
                {
                    Debug.DrawRay(target.centrePoint.position, (weapon.projectileSpawn.position - target.centrePoint.position), Color.green);
                    if(hit.transform.root == target.transform.root)
                    {
                        weapon.StartFiring();
                        return;
                    }
                }
            }
            else
            {
                Debug.DrawRay(weapon.projectileSpawn.position, (target.centrePoint.position - weapon.projectileSpawn.position) * 1000, Color.black);
            }
                        
            weapon.StopFiring();
        }

        else
        {
            weapon.StopFiring();
        }
    }

    void CheckCurrentTargetValid()
    {
        if(target != null)
        {
            if(Vector3.Distance(target.transform.position,transform.position) > searchRadius)
            {
                target = null;
                Debug.Log("Current target invalidated");
            }           
        }
    }

    void CheckTargetAlive()
    {
        if(target != null)
        {
            if(target.state == Player.playerState.Dead)
            {
                target = null;
                Debug.Log("Current target be dead");
            }   
        }
    }

    void TargetEnemy()
    {
        if (target != null)
        {
            Vector3 targetDir = (target.transform.position + aimOffset) - transform.position;
            targetDir = targetDir.normalized;

            Vector3 currentDir = transform.forward;

            currentDir = Vector3.RotateTowards(currentDir, targetDir, turnRateRadians*Time.deltaTime, 1.0f);
            currentDir.y = Mathf.Clamp(currentDir.y, -maxRotation, maxRotation);

            Quaternion qDir = new Quaternion();
            qDir.SetLookRotation(currentDir, Vector3.up);
            transform.rotation = qDir;
        }
    }

    private void GetPlayerTarget()
    {

        Collider[] hits = Physics.OverlapSphere(transform.position, searchRadius, searchLayers);
        {
            float distance = Mathf.Infinity;
            Transform closest = null;

            foreach(Collider hit in hits)
            {
                
                if(hit.transform.root == this.transform.root)
                {
                    continue;
                }

                if(hit.GetComponent<Player>() && hit.GetComponent<Player>().state == Player.playerState.Dead)
                {
                    continue;
                }
                
                Vector3 diff = (hit.transform.position - transform.position);
                var curDistance = diff.sqrMagnitude;
                if (curDistance < distance)
                {
                    distance = curDistance;
                    closest = hit.transform;
                }
            }
            if(closest != null)
            {
                target = closest.transform.root.GetComponent<Player>();
                Debug.Log("Found new target: " + target);
            }
                
        } 
    }

    public void NewTargetAcquired(Player newTarget)
    {
        Debug.Log("AutoAim: New target received");
        target = newTarget;
    }

    public void TargetLost()
    {
        Debug.Log("AutoAim: Target lost");
        target = null;  
    }

    public void SetAutoFindTarget(bool val)
    {
        autoFindTarget = val;
    }

    private void OnDrawGizmos() 
    {
        Gizmos.DrawWireSphere(transform.position, searchRadius);
        if(target!=null)
        {
            Gizmos.DrawWireSphere(target.transform.position, 5f);
        }

        if(weapon)
        {
            Gizmos.DrawWireSphere(weapon.projectileSpawn.position, .5f);
        }
        
    }
}
