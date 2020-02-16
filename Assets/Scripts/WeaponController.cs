using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] BaseWeapon frontWeapon;
    [SerializeField] BaseWeapon rearWeapon;
    [SerializeField] BaseWeapon leftWeapon;
    [SerializeField] BaseWeapon rightWeapon;

    [SerializeField] WeaponUIUpdate weaponUIUpdate;

    private BaseWeapon selectedWeapon;
    public enum Weapon
    {
        front,
        rear,
        left,
        right
    }

    private void Start() 
    {
        weaponUIUpdate =  GameObject.FindObjectOfType<WeaponUIUpdate>();
        WeaponRegister();
    }

    private void WeaponRegister()
    {
        // Scan through weapon options
        // Assign first found to current weapon
        // Register all available weapons with UI

        bool found = false;
        List<BaseWeapon> wList = new List<BaseWeapon>()
        {
            frontWeapon,
            rearWeapon, 
            leftWeapon,
            rightWeapon
        };
        List<BaseWeapon> availableWeapons = new List<BaseWeapon>();

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

        weaponUIUpdate.RegisterWeapons(availableWeapons);

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
                if(rearWeapon) 
                    selectedWeapon = rearWeapon;
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
        if(weaponUIUpdate != null)
        {
            weaponUIUpdate.SetCurrentWeapon(weaponNumber);
        }
    }
}
