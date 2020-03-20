using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OLDWeaponController : MonoBehaviour
{
    [SerializeField] BaseWeapon frontWeapon;
    [SerializeField] BaseWeapon topWeapon;
    [SerializeField] BaseWeapon leftWeapon;
    [SerializeField] BaseWeapon rightWeapon;
    List<BaseWeapon> availableWeapons;
    [SerializeField] WeaponUIUpdate weaponUIUpdate;

    private bool IsAI = false;
    [SerializeField] bool enableAutoAim = false;

    private BaseWeapon selectedWeapon;
    public enum Weapon
    {
        front,
        top,
        left,
        right
    }

    // USed to be Player A c t ive
    // public void dfsfsdfsdfsdfs() 
    // {
    //     if(weaponUIUpdate)
    //     {
    //         weaponUIUpdate =  GameObject.FindObjectOfType<WeaponUIUpdate>();    
    //     }
    //     WeaponRegister();
    //     SetupAutoFindOnWeapons();
    // }

    private void WeaponRegister()
    {        
        // Scan through weapon options
        // Assign first found to current weapon
        // Register all available weapons with UI

        bool found = false;
        List<BaseWeapon> wList = new List<BaseWeapon>()
        {
            frontWeapon,
            topWeapon, 
            leftWeapon,
            rightWeapon
        };

        availableWeapons = new List<BaseWeapon>();

        foreach (BaseWeapon w in wList)
        {
            if(w != null)
            {
                availableWeapons.Add(w);
                if(found == false)
                {
                    selectedWeapon = w;
                    found = true;
                }                
            }
        }

        if(!IsAI && weaponUIUpdate)
        {
            weaponUIUpdate.RegisterWeapons(availableWeapons);
        }

        if(!found)
        {
            Debug.Log("No weapon found on vehicle");
        }
    }


    public BaseWeapon SelectWeapon(int weaponChoice)
    {
        // Ensure current weapon stops first
        selectedWeapon.StopFiring();
        
        switch (weaponChoice)
        {
            case 1:
                if(frontWeapon)
                    selectedWeapon = frontWeapon;
                    UpdateUIComponent(1);
                break;
                
            case 2:
                if(topWeapon) 
                    selectedWeapon = topWeapon;
                    UpdateUIComponent(2);
                break;

            case 3:
                if(leftWeapon)
                    selectedWeapon = leftWeapon;
                    UpdateUIComponent(3);
                break;

            case 4:
                if(rightWeapon)
                    selectedWeapon = rightWeapon;
                    UpdateUIComponent(4);
                break;
            
            default:
                return null;
        }
        return selectedWeapon;
    }


    public void Fire()
    {   
        if(selectedWeapon != null)
        {
            selectedWeapon.StartFiring();
        }
        
    }

    public void StopFiring()
    {
        if(selectedWeapon != null)
        {
            selectedWeapon.StopFiring();
        }
        
    }

    private void UpdateUIComponent(int weaponNumber)
    {
        if(weaponUIUpdate)
        {
            weaponUIUpdate.SetCurrentWeapon(weaponNumber);
        }
    }

    public void RegisterAsAI()
    {
        IsAI = true;
    }

    public void SetupAutoFindOnWeapons()
    {
        // Scan through all available weapons and allow auto aim if AI
    
        foreach(BaseWeapon bw in availableWeapons)
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
}
