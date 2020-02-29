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
    [SerializeField] GameObject reticleCanvas;
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
        reticleCanvas.SetActive(true);
        state = lockState.NoLock;
    }   

    
    private void LateUpdate()
    {
        // Rotate collider same direction as camera
        // transform.rotation = cam.transform.rotation;
        transform.eulerAngles =  new Vector3(0, cam.transform.eulerAngles.y, cam.transform.eulerAngles.z);

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
            lockOnIndicator.transform.LookAt(cam.transform);
    
            lockOnIndicator.transform.position += lockOnIndicator.transform.forward *1.5f;
        }
        else
        {
            possibleTargets.Remove(lockedTarget);
        }
    }

    public void InitateLockOn()
    {
        RemoveLock();
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

    public void RemoveLock()
    {
        lockedTarget = null;
        lockOnIndicator.SetActive(false);
        state = lockState.NoLock;
        animator.SetBool("acquiring", false);
        animator.SetBool("acquired", false);
        transform.root.BroadcastMessage("TargetLost");
    }
    
    private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.layer == 8) //Player
        {
            if(!possibleTargets.Contains(other.transform.root.GetComponent<Player>()))
            {
                possibleTargets.Add(other.transform.root.GetComponent<Player>());
            }
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if(other.gameObject.layer == 8) //Player
        {
            Player x = other.transform.root.GetComponent<Player>();
            possibleTargets.Remove(x);
        }
    }
}
