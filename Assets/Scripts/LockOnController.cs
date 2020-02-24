using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class LockOnController : MonoBehaviour
{
    [SerializeField] private Camera cam;
    private bool enableLockOn;
    private BoxCollider visionCollider;

    [SerializeField] List<Player> possibleTargets = new List<Player>();
    [SerializeField] Player lockedTarget;
    [SerializeField] GameObject lockOnIndicatorPrefab;
    GameObject lockOnIndicator;
    Animator animator;

    private enum lockState {
        NoLock,
        Acquiring,
        Acquired,
        Locked
    }

    private lockState state;

    public void PlayerActive()
    {
        Debug.Log("Lockoncontroller: Player active called");
        visionCollider = GetComponent<BoxCollider>();
        if(lockOnIndicator == null)
        {
            lockOnIndicator = Instantiate(lockOnIndicatorPrefab);
            animator = lockOnIndicator.GetComponent<Animator>();
            animator.Rebind();
            lockOnIndicator.SetActive(false);
            
        }
        state = lockState.NoLock;
    }   

    
    private void LateUpdate()
    {
        // Rotate collider same direction as camera
        transform.rotation = cam.transform.rotation;

        if(state == lockState.Acquiring)
        {
            if(possibleTargets.Count > 0 && lockedTarget == null)
            {
                lockedTarget = possibleTargets[0];
                lockOnIndicator.SetActive(true);
                state = lockState.Acquired;
                // Enable acquiring animation
            }
            animator.SetBool("acquiring", true);
            
        }

        else if(state == lockState.Acquired)
        {
            // umm?
        }
        else if(state == lockState.Locked)
        {
            // umm2?
        }

        // Update lockOnIndicator position/rotation
        if(lockedTarget != null)
        {
            lockOnIndicator.transform.position = lockedTarget.transform.position;
            lockOnIndicator.transform.LookAt(this.transform);
            lockOnIndicator.transform.position += Vector3.forward *1.5f;
        }
    }

    public void InitateLockOn()
    {
        CancelLock();
        state = lockState.Acquiring;
    }

    public void LockOn()
    {
        if(state == lockState.Acquired)
        {
            transform.root.BroadcastMessage("NewTargetAcquired", lockedTarget);
            animator.SetBool("acquired", true);
        }
    }

    public void CancelLock()
    {
        lockedTarget = null;
        lockOnIndicator.SetActive(false);
        state = lockState.NoLock;
        animator.SetBool("acquiring", false);
        animator.SetBool("acquired", false);

    }

    public void RemoveLock()
    {
        CancelLock();
        transform.root.BroadcastMessage("TargetLost");
    }
    
    private void OnTriggerEnter(Collider other) 
    {
        if(other.transform.tag == "Player")
        {
            possibleTargets.Add(other.GetComponent<Player>());
        }
        
    }

    private void OnTriggerExit(Collider other) 
    {
        if(other.transform.tag == "Player")
        {
            Player x = other.GetComponent<Player>();
            possibleTargets.Remove(x);
            if(x == lockedTarget)
            {
                lockedTarget = null;
                CancelLock();
            }
        }
    }
}
