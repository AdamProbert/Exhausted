using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using NodeCanvas.StateMachines;
using NodeCanvas.Framework;

public class PlayerEventManager : MonoBehaviour
{
    // Actions
    public Action<GameManager.GameState> OnGameStateChanged = delegate{};
    public Action<float> OnPlayerHealthChanged = delegate{};
    public Action<float> OnPlayerArmourChanged = delegate{};

    public Action<Player.state> OnPlayerStateChanged = delegate{};

    public Action<BaseWeapon.status> OnWeaponStatusChange = delegate{};

    public Action<Player> OnNavTargetChange = delegate{};

    public Action<Player> OnWeaponTargetChange = delegate{};

    public Action<Pickup> OnPickupItem = delegate{};

    public Action<Player, Player.state> GlobalPlayerStateChange = delegate{};

    public Action<Player, Waypoint> GlobalPlayerReachedWaypoint = delegate{};

    public Action<Player, int> GlobalRacePositionChanged = delegate{};

    public Action OnRaceAheadOfPlayer = delegate{};
    public Action OnRaceBehindPlayer = delegate{};

    public Action<bool> OnPlayerBoost = delegate{};

    public Action OnVehicleLoad = delegate{};

    // Node canvas
    private FSMOwner owner;

    // Start is called before the first frame update
    void Awake()
    {
        if(GetComponent<FSMOwner>())
        {
            owner = GetComponent<FSMOwner>();
        }   
    }

    void HandleGameStateChange()
    {
        if(OnGameStateChanged != null)
        {
            OnGameStateChanged(GameManager.Instance.gameState);
        }
    }

    public void HandleWaypointReachedForAI(Player player, Waypoint waypoint)
    {
        if(owner != null)
        {
            owner.SendEvent<Waypoint>("WaypointReached", waypoint, this);
        }
    }

    public void HandlePickupAI(Pickup p)
    {
        if(owner != null)
        {
            // owner.SendEvent<Pickup>("PickupReached", p, this);
            Graph.SendGlobalEvent("PickupReached", p, this);
        }
    }

    private void OnEnable() 
    {
        // Listen for game state changes and propogate down
        GameManager.Instance.OnStateChange += HandleGameStateChange;
        GlobalPlayerReachedWaypoint += HandleWaypointReachedForAI;
        OnPickupItem += HandlePickupAI;
    }

    private void OnDisable() 
    {
        GlobalPlayerReachedWaypoint -= HandleWaypointReachedForAI;    
        OnPickupItem -= HandlePickupAI;
    }
}
