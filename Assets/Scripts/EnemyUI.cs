using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
    Player player;
    PlayerEventManager playerEventManager;
    [SerializeField] Image playerHealth;
    [SerializeField] Image playerArmour;
    [SerializeField] Canvas canvas;
    Camera cam;
    
    // Start is called before the first frame update
    void Awake()
    {
        player = GetComponent<Player>();
        playerEventManager = GetComponent<PlayerEventManager>();
    }

    private void Start() 
    {
        cam = GameObject.Find("UICamera").GetComponent<Camera>();
        canvas.worldCamera = cam;
    }

    private void LateUpdate() 
    {
        if(cam != null)
        {
            canvas.transform.LookAt(cam.transform.position);
        }
    }

    private void OnEnable() 
    {
        playerEventManager.OnPlayerArmourChanged += HandlePlayerArmourChange;
        playerEventManager.OnPlayerHealthChanged += HandlePlayerHealthChange;
        playerEventManager.OnPlayerStateChanged += HandlePlayerStateChange;
    }

    private void OnDisable() 
    {
        playerEventManager.OnPlayerArmourChanged += HandlePlayerArmourChange;
        playerEventManager.OnPlayerHealthChanged += HandlePlayerHealthChange;
        playerEventManager.OnPlayerStateChanged -= HandlePlayerStateChange;
    }

    public void HandlePlayerStateChange(Player.state newstate)
    {
        if(newstate != Player.state.Alive)
        {
            canvas.enabled = false;
        }
    }

    public void HandlePlayerHealthChange(float x)
    {
        playerHealth.fillAmount = player.currentHealth / player.maxHealth;
    }

    public void HandlePlayerArmourChange(float x)
    {
        playerArmour.fillAmount = player.currentArmour / player.maxArmour;
    }

}
