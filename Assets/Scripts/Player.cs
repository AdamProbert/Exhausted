using UnityEngine;
using NodeCanvas.StateMachines;
using NodeCanvas;

[RequireComponent(typeof(WeaponController))]
[RequireComponent(typeof(PlayerEventManager))]
public class Player : MonoBehaviour
{

    // Player events -- all events should route through here
    PlayerEventManager playerEventManager;

    [Header("Setup")]
    private AIInput2 aIInput;
    public bool isAI = true;
    public bool testMode = false;

    [SerializeField] public Transform centrePoint;

    [Header("Health")]
    [SerializeField] float maxHealth;
    [SerializeField] float m_currentHealth;
    [SerializeField] public float currentHealth{
        get{return m_currentHealth;}
        set{
            if(m_currentHealth == value) return;
            m_currentHealth = value;
        }
    }
    private RectTransform healthBar;
    private Vector2 ogHealhBarSize;

    [Header("Armour")]
    [SerializeField] public float maxArmour;
    [SerializeField] float m_currentArmour;
    [SerializeField] public float currentArmour{ // Initally set from ArmourManager
        get{return m_currentArmour;}
        set{
            if(m_currentArmour == value) return;
            m_currentArmour = value;
            if(m_currentArmour > maxArmour) m_currentArmour = maxArmour;
        }
    }

    private RectTransform armourBar;
    private Vector2 ogArmourSize;

    
    public enum state{
        Inactive,
        Building,
        Alive,
        Dead
    };

    [SerializeField] private state m_state;
    public state playerState{
        get{return m_state;}
        set{
            m_state = value;
            playerEventManager.OnPlayerStateChanged(m_state);
            playerEventManager.GlobalPlayerStateChange(this, m_state);
        }
    }

    private void Awake() 
    {
        // Listeners
        playerEventManager = GetComponent<PlayerEventManager>();
        playerEventManager.OnGameStateChanged += HandleGameStateChange;
        playerEventManager.OnPlayerStateChanged += HandlePlayerStateChange;
        playerEventManager.OnPlayerArmourChanged += ArmourChangeHandler;
        playerEventManager.OnPlayerHealthChanged += HealthChangeHandler;
    }

    private void Start() 
    {
        // Set health    
        currentHealth = maxHealth;

        // Is us AI?
        if(GetComponent<AIInput2>() && !GetComponent<UserInput>())
        {
            isAI = true;
        }
        else if(!GetComponent<AIInput2>() && GetComponent<UserInput>())
        {
            isAI = false;      
        }
        else
        {
            Debug.LogError("!Player must have either AIInput script or UserInput!");
        }

        Debug.Log("Player state set in awake to: " + playerState);
    }

    private void HealthChangeHandler(float healthDiff)
    {    
        currentHealth += healthDiff;        
        UpdateHealthUI();
        if(currentHealth <= 0)
        {
            playerState = state.Dead; 
        }
    }

    private void UpdateHealthUI()
    {
        if(healthBar)
        {
            float percentHealth = (currentHealth/maxHealth) * ogHealhBarSize.x;
            healthBar.sizeDelta = new Vector2(percentHealth, healthBar.sizeDelta.y);
        }
    }

    public void SetupArmour(float armour)
    {
        maxArmour = armour;
        currentArmour = armour;
        UpdateArmourUI();
    }

    private void ArmourChangeHandler(float armourDifference)
    {
        currentArmour += armourDifference;
        UpdateArmourUI();
    }

    private void UpdateArmourUI()
    {
        if(armourBar)
        {
            float percent = (currentArmour/maxArmour) * ogArmourSize.x;
            armourBar.sizeDelta = new Vector2(percent, armourBar.sizeDelta.y);
        }
    }

    public void TakeDamage(float damage)
    {
        if(playerState == state.Alive)
        {
            if(currentArmour > 0)
            {
                float remainder = currentArmour - damage;
                if(remainder >= 0)
                {
                    playerEventManager.OnPlayerArmourChanged(-damage);    
                }
                else
                {
                    playerEventManager.OnPlayerArmourChanged(-currentArmour);
                    playerEventManager.OnPlayerHealthChanged(-remainder);
                }
            }
            else
            {
                playerEventManager.OnPlayerHealthChanged(-damage);
            }
        }
    }

    public void HandleGameStateChange(GameManager.GameState newstate)
    {
        Debug.Log("Game state changed to: " + newstate);
        if(newstate== GameManager.GameState.BUILD)
        {
            playerState=state.Building;
        }

        if(newstate == GameManager.GameState.PLAY)
        {
            playerState=state.Alive;            
        }
    }

    private void HandlePlayerStateChange(state newstate)
    {
        Debug.Log("PLAYER: Player state changed to: " + newstate);
        if(newstate == Player.state.Building || newstate == Player.state.Inactive)
        {
            if(!isAI)
            {
                GetComponent<UserInput>().Deactivate();
            }
            else
            {
                GetComponent<AIInput2>().enabled = false;
                GetComponent<FSMOwner>().enabled = false;
            }
        }

        if(newstate == Player.state.Alive)
        {
            if(!isAI)
            {
                if(!healthBar & !testMode)
                {
                    healthBar = GameObject.Find("HealthForeground").GetComponent<RectTransform>();
                    ogHealhBarSize = healthBar.sizeDelta;
                }

                if(!armourBar & !testMode)
                {
                    armourBar = GameObject.Find("ArmourForeground").GetComponent<RectTransform>();
                    ogArmourSize = armourBar.sizeDelta;
                }      
            }
            else
            {
                GetComponent<AIInput2>().enabled = true;
                GetComponent<FSMOwner>().enabled = true;
            }
                    
        }

        if(newstate == Player.state.Dead)
        {
            if(isAI)
            {
                GetComponent<FSMOwner>().enabled = false;
                GetComponent<AIInput2>().enabled = false;
            }
        }
    }
}
