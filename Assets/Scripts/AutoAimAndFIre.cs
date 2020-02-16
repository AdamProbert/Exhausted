using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BaseWeapon))]
public class AutoAimAndFIre : MonoBehaviour
{
    private bool autoFindTarget;
    [SerializeField][Range(0,180)] protected float maxRotation;
    [SerializeField][Range(.1f, 10f)] protected float rotationSpeed;
    [SerializeField][Range(5f, 100f)] private float searchRadius;
    [SerializeField]private LayerMask searchLayers;

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
            CheckCurrentTargetValid();
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
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(transform.position, target.transform.position - transform.position, out hit, Mathf.Infinity, searchLayers))
            {
                Debug.DrawRay(transform.position, (target.transform.position - transform.position) * hit.distance, Color.yellow);
                weapon.StartFiring();
                // Debug.Log("Did Hit");
            }
            else
            {
                Debug.DrawRay(transform.position, (target.transform.position - transform.position) * 1000, Color.white);
                // Debug.Log("Did not Hit");
            }
        }
    }

    void CheckCurrentTargetValid()
    {
        if(target != null)
        {
            if(Vector3.Distance(target.transform.position,transform.position) > searchRadius)
            {
                target = null;
            }
            else if(target.state == Player.playerState.Dead)
            {
                target = null;
            }
        }
    }

    void TargetEnemy()
    {
        
        if (target == null)
        {
            GetPlayerTarget();
        }

            

        if (target != null)
        {
            Vector3 targetDir = target.transform.position - transform.position;
            targetDir = targetDir.normalized;

            Vector3 currentDir = transform.forward;

            currentDir = Vector3.RotateTowards(currentDir, targetDir, turnRateRadians*Time.deltaTime, 1.0f);

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
                
                if(hit.transform.root == transform.root)
                {
                    continue;
                }

                if(hit.GetComponent<Player>() && hit.GetComponent<Player>().state == Player.playerState.Dead)
                {
                    continue;
                }
                
                Debug.Log("Autoaim found someone to hit");
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
                Debug.Log("Set target to: " + target);
            }
                
        } 
    }

    public void NewTargetAcquired(Player newTarget)
    {
        Debug.Log("AutoAim: New target received");
        target = newTarget;
    }

    public void SetAutoFindTarget(bool val)
    {
        autoFindTarget = val;
    }

    private void OnDrawGizmos() 
    {
        Gizmos.DrawWireSphere(transform.position, searchRadius);
    }
}
