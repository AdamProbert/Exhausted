using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockOnController : MonoBehaviour
{
    PlayerEventManager playerEventManager;
    [SerializeField] private Camera cam;
    private bool enableLockOn;
    [SerializeField] List<Player> allEnemies = new List<Player>();
    [SerializeField] LayerMask layermask;
    [SerializeField] Player lockedTarget;
    [SerializeField] Sprite lockingOnImage;
    [SerializeField] Sprite lockedImage;

    [SerializeField] AudioClip lockOnSound;
    Image lockOnIndicator;
    Animator animator;
    AudioSource audioSource;


    private enum lockState {
        NoLock,
        Acquiring,
        Acquired,
        Locked
    }

    private lockState state;

    private void Awake() 
    {
        audioSource = GetComponent<AudioSource>();
        playerEventManager = transform.root.GetComponent<PlayerEventManager>();
        lockOnIndicator = GameObject.Find("LockOnTargetImage").GetComponent<Image>();
        lockOnIndicator.enabled = false;
    }

    private void OnEnable() 
    {
        playerEventManager.OnPlayerStateChanged += HandlePlayerStateChange;    
    }

    private void OnDisable() 
    {
        playerEventManager.OnPlayerStateChanged -= HandlePlayerStateChange;
    }

    private void Start() 
    {
        Player[] possiblePlayers = GameObject.FindObjectsOfType<Player>(); 
        foreach (Player player in possiblePlayers)
        {
            if(player.transform.root != this.transform.root)
            {
                allEnemies.Add(player);
                player.GetComponent<PlayerEventManager>().GlobalPlayerStateChange += RemoveEnemy;
            }
        }
    }

    public void HandlePlayerStateChange(Player.state newstate)
    {
        if(newstate == Player.state.Alive)
        {
            if(lockOnIndicator == null)
            {
                // animator = lockOnIndicator.GetComponent<Animator>();
                // animator.Rebind();
                // lockOnIndicator.SetActive(false);       
            }
            state = lockState.NoLock;
        }
    }   

    private void LateUpdate() 
    {
        if(lockedTarget && lockedTarget.playerState != Player.state.Alive)
        {
            RemoveLock();
            return;
        }
    }
    private void FixedUpdate()
    {
        // Update lockOnIndicator position/rotation
        if(lockedTarget != null)
        {
            lockOnIndicator.transform.position = Vector3.Lerp(lockOnIndicator.transform.position, cam.WorldToScreenPoint(lockedTarget.transform.position), .5f);
        }
    }

    private Player GetTarget()
    {
        // Loop over every enemy, find the angle difference from the camera centre point to that enemy
        // Check we can see them
        // Return the best target
        RaycastHit hit;
        float bestTargetValue = -1;
        Player bestTarget = null;
        Vector3 cameraDir = cam.transform.forward;
        
        foreach (Player enemy in allEnemies)
        {
            Vector3 screenPoint = cam.WorldToViewportPoint(enemy.centrePoint.transform.position);
            bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
            if(!onScreen)
            {
                continue;
            }

            Vector3 enemyDir = enemy.centrePoint.transform.position - cam.transform.position;
            // Does the ray ientersect any objects?
            if (Physics.Raycast(cam.transform.position, enemyDir, out hit, Mathf.Infinity, layermask))
            {
                if(hit.transform.root.gameObject == enemy.gameObject)
                {
                    float dotProduct = Vector3.Dot(cameraDir.normalized, enemyDir.normalized); // -1 to 1 face same direction to opposite directions
                    if(dotProduct > bestTargetValue)
                    {
                        bestTarget = enemy;
                        bestTargetValue = dotProduct;
                    }
                }
            }
        }
        return bestTarget;
    }

    private void RemoveEnemy(Player enemy, Player.state newstate)
    {
        // Listen for enemy player deaths and remove from list
        if(newstate == Player.state.Dead)
        {
            allEnemies.Remove(enemy);
        }
    }

    public void InitateLockOn()
    {
        Debug.Log("Initiate lock called");
        if(lockedTarget != null)
        {
            RemoveLock();
        }
        lockedTarget = GetTarget();
        if(lockedTarget)
        {
            lockOnIndicator.enabled = true;
            lockOnIndicator.sprite = lockingOnImage;
            // animator.SetBool("acquiring", true);
            state=lockState.Acquiring;
        }
    }

    public void LockOn()
    {
        Debug.Log("Lock on called");
        if(state == lockState.Acquiring)
        {
            playerEventManager.OnWeaponTargetChange(lockedTarget);
            // animator.SetBool("acquired", true);
            audioSource.PlayOneShot(lockOnSound);
            lockOnIndicator.sprite = lockedImage;
            state=lockState.Acquired;
        }
    }

    public void RemoveLock()
    {
        Debug.Log("Remove lock called");
        lockedTarget = null;
        lockOnIndicator.sprite = null;
        lockOnIndicator.enabled = false;
        state = lockState.NoLock;
        // animator.SetBool("acquiring", false);
        // animator.SetBool("acquired", false);
        playerEventManager.OnWeaponTargetChange(null);
    }
}
