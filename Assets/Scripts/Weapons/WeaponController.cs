﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    WeaponUIUpdate weaponUIUpdate;
    [SerializeField][Tooltip("Will auto find if 0 length")] List<BaseWeapon> weapons = new List<BaseWeapon>();

    private bool IsAI = false;
    [SerializeField] bool enableAutoAim = false;

    public void PlayerActive() 
    {
        Debug.Log("Weapon controller: Player active called");
        if(!weaponUIUpdate)
        {
            weaponUIUpdate =  GameObject.FindObjectOfType<WeaponUIUpdate>();    
        }
        if(weapons.Count == 0)
        {
            FindWeapons();
        }
        WeaponRegister();
        SetupAutoFindOnWeapons();
    }

    public void RegisterAsAI()
    {
        IsAI = true;
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
                if(enableAutoAim)
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

    public void PlayerDied()
    {
        foreach(BaseWeapon w in weapons)
        {
            w.gameObject.SetActive(false);
        }
    }
}
