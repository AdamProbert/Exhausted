using UnityEngine;
using NodeCanvas.StateMachines;

[RequireComponent(typeof(WeaponController))]
public class Player : MonoBehaviour
{
    [Header("Setup")]
    private WeaponController weaponController;
    private AIInput2 aIInput;
    public bool isAI = true;

    public bool testMode = false;

    [Header("Health")]
    [SerializeField] float maxHealth;
    [SerializeField] float m_currentHealth;

    private RectTransform healthBar;
    private Vector2 ogHealhBarSize;

    [SerializeField] public float currentHealth{
        get{return m_currentHealth;}
        set{
            if(m_currentHealth == value) return;
            m_currentHealth = value;
            if(OnHealthChange != null)
            {
                OnHealthChange(m_currentHealth);
            }
        }
    }

    public delegate void OnHealthChangeDelegate(float newVal);
    public event OnHealthChangeDelegate OnHealthChange;
    
    [SerializeField] private playerState m_state;
    public playerState state{
        get{return m_state;}
        set{
            if(m_state == value) return;
            m_state = value;
            if(OnStateChange != null)
            {
                OnStateChange(m_state, this);
            }
        }
    }

    public enum playerState{
        Inactive,
        Building,
        Alive,
        Dead
    };

    public delegate void OnStateChangeDelegate(playerState newState, Player thisPlayer);
    public event OnStateChangeDelegate OnStateChange;


    private void Awake()
    {
        GameManager.Instance.OnStateChange += HandleGameStateChange;    
        this.OnStateChange += HandlePlayerStateChange;

        if(GameManager.Instance.gameState == GameManager.GameState.BUILD)
        {
            state=playerState.Building;    
        }
        else if(GameManager.Instance.gameState == GameManager.GameState.PLAY)
        {
            state=playerState.Alive;    
        }

        Debug.Log("Player state set in awake to: " +state);

        currentHealth = maxHealth;
        this.OnHealthChange += StatusHandler;
        weaponController = GetComponent<WeaponController>();

        // Is us AI?
        if(GetComponent<AIInput2>() && !GetComponent<UserInput>())
        {
            isAI = true;
            aIInput = GetComponent<AIInput2>();
            aIInput.OnTargetChange += OnNavTargetChange;
            weaponController.RegisterAsAI();
        }
        else if(!GetComponent<AIInput2>() && GetComponent<UserInput>())
        {
            isAI = false;      
        }
        else
        {
            Debug.LogError("!Player must have either AIInput script or UserInput!");
        }
    }

    private void OnNavTargetChange(Player newTarget)
    {
        // Broadcast message to all components on this game object
        if(newTarget != null)
        {
            BroadcastMessage("NewTargetAcquired", newTarget);    
        }
        else
        {
            BroadcastMessage("TargetLost");    
        }
        Debug.Log("Player: On nav target change received");
    }

    private void StatusHandler(float newHealth)
    {
        if(state == playerState.Alive)
        {
            if(healthBar)
            {
                float percentHealth = (newHealth/maxHealth) * ogHealhBarSize.x;
                healthBar.sizeDelta = new Vector2(percentHealth, healthBar.sizeDelta.y);
            }

            if(newHealth <= 0)
            {
                KillPlayer();
                
            }
        }
        
    }

    private void KillPlayer()
    {
        state = playerState.Dead;
        // Deactivate input
        if(isAI)
        {
            GetComponent<FSMOwner>().enabled = false;
            GetComponent<AIInput2>().enabled = false;
        }
        else
        {
            GetComponent<UserInput>().enabled = false;
        } 
       
        BroadcastMessage("PlayerDied"); // Player removal handled in DestroyCar
    }

    // Old collision based damage
    // private void OnCollisionEnter(Collision other) 
    // {   
    //     // if projectile hit us
    //     if(other.gameObject.layer == 10)
    //     {
    //         float forceHit = other.relativeVelocity.magnitude;
    //         forceHit *= other.rigidbody.mass;

    //         //Debug.Log("Player: " + gameObject.name + " hit for " + forceHit + " damage");

    //         if(state == playerState.Alive)
    //         {
    //             currentHealth -= forceHit;
    //         }
            
    //     }    
    // }

    public void TakeDamage(float damage)
    {
        // TODO: Apply armour modifiers
        if(state == playerState.Alive)
        {
            currentHealth -= damage;
        }
    }

    public void HandleGameStateChange()
    {
        Debug.Log("Game state changed to: " + GameManager.Instance.gameState);
        if(GameManager.Instance.gameState == GameManager.GameState.BUILD)
        {
            state=playerState.Building;
        }

        if(GameManager.Instance.gameState == GameManager.GameState.PLAY)
        {
            state=playerState.Alive;            
        }
    }

    private void HandlePlayerStateChange(playerState newstate, Player player)
    {
        Debug.Log("Player state changed to: " + newstate);
        if(newstate == Player.playerState.Building)
        {
            if(!isAI)
            {
                GetComponent<UserInput>().Deactivate();
                transform.Find("Camera").gameObject.SetActive(false);
            }
        }

        if(newstate == Player.playerState.Alive)
        {
            if(!isAI)
            {
                if(GetComponent<UserInput>())
                {
                    GetComponent<UserInput>().SetActive();
                }

                transform.Find("Camera").gameObject.SetActive(true);
                if(!healthBar)
                {
                    healthBar = GameObject.Find("Foreground").GetComponent<RectTransform>();
                    ogHealhBarSize = healthBar.sizeDelta;
                }      
            }        
            BroadcastMessage("PlayerActive");
        }
    }
}
