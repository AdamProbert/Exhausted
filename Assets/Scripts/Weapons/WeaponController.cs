using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    PlayerEventManager playerEventManager;
    WeaponUIUpdate weaponUIUpdate;
    [SerializeField][Tooltip("Will auto find if 0 length")] List<BaseWeapon> weapons = new List<BaseWeapon>();

    private bool IsAI = false;
    [SerializeField] bool enableAutoAim = false;

    private void Start() 
    {
        playerEventManager = GetComponent<PlayerEventManager>();
        playerEventManager.OnPlayerStateChanged += HandlePlayerStateChange;    
    }

    public void HandlePlayerStateChange(Player.state newstate) 
    {
        Debug.Log("WeaponCopntroller: Player state set to: " + newstate);
        if(newstate == Player.state.Alive)
        {
            if(!weaponUIUpdate)
            {
                weaponUIUpdate =  GameObject.FindObjectOfType<WeaponUIUpdate>();    
            }

            if(weapons.Count == 0)
            {
                FindWeapons();
            }

            if(!GetComponent<Player>().isAI)
            {
                WeaponRegister();
                IsAI = false;
            }
            else
            {
                IsAI = true;
            }

            foreach(BaseWeapon w in weapons)
            {
                w.currentStatus = BaseWeapon.status.Active;
            }

            SetupAutoFindOnWeapons();
        }

        else if(newstate != Player.state.Alive)
        {
            foreach(BaseWeapon w in weapons)
            {
                w.currentStatus = BaseWeapon.status.Inactive;
            }
        }
    }
    private void FindWeapons()
    {
        weapons.AddRange(GetComponentsInChildren<BaseWeapon>());
    }

    public void AddWeapon(BaseWeapon weapon)
    {
        weapons.Add(weapon);
    }

    public void RemoveWeapon(BaseWeapon weapon)
    {
        weapons.Remove(weapon);
    }

    private void WeaponRegister()
    {        
        if(!IsAI & weaponUIUpdate!=null)
        {
            weaponUIUpdate.RegisterWeapons(weapons);
        }

        if(weapons.Count == 0)
        {
            Debug.Log("No weapon found on vehicle");
        }
    }

    public void SetupAutoFindOnWeapons()
    {
        // Scan through all available weapons and allow auto aim if AI
    
        foreach(BaseWeapon bw in weapons)
        {
            if(bw.GetComponent<AutoAimAndFIre>())
            {
                if(enableAutoAim || IsAI)
                {
                    bw.GetComponent<AutoAimAndFIre>().SetAutoFindTarget(true);
                }
                else
                {
                    bw.GetComponent<AutoAimAndFIre>().SetAutoFindTarget(false);
                }
                
            }
        }
    }
}
