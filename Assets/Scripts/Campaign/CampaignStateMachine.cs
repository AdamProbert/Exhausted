using UnityEngine;

public class CampaignStateMachine : Singleton<CampaignStateMachine> 
{

    
    public enum CampaignState { DEFAULT, LANDMARK }

    public delegate void OnStateChangeHandler();
    public event OnStateChangeHandler OnCampaignStateChange;

    protected CampaignStateMachine() {}
    [SerializeField] public CampaignState state { get; private set; }

    private static CampaignStateMachine stateMachine;
 
    public static CampaignStateMachine instance
    {
        get
        {
            if (!stateMachine)
            {
                stateMachine = FindObjectOfType(typeof(CampaignStateMachine)) as CampaignStateMachine;
                if (!stateMachine)
                {
                    Debug.LogError("There needs to be one active EventManger script on a GameObject in your scene.");
                }
 
                else
                {
                    stateMachine.Init();
                }
            }
            return stateMachine;
        }
    }

    void Init()
    {
        state = CampaignState.DEFAULT;
    } 
    
    public void SetState(CampaignState state){
        Debug.Log("CampaignStateMachine set state called. New state: " + state);
        if(state != this.state)
        {
            this.state = state;
            if(OnCampaignStateChange != null)
            {
                OnCampaignStateChange();
            }   
        }
    }
}