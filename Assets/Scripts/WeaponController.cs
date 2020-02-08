using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] BaseWeapon frontWeapon;
    [SerializeField] BaseWeapon rearWeapon;
    [SerializeField] BaseWeapon leftWeapon;
    [SerializeField] BaseWeapon rightWeapon;

    private BaseWeapon selectedWeapon ;
    public enum Weapon
    {
        front,
        rear,
        left,
        right
    }

    private void Start() 
    {
        SetInitialWeapon();
    }

    private void SetInitialWeapon()
    {
        bool found = false;
        List<BaseWeapon> wList = new List<BaseWeapon>()
        {
            frontWeapon,
            rearWeapon, 
            leftWeapon,
            rightWeapon
        };

        foreach (BaseWeapon w in wList)
        {
            if(w != null)
            {
                selectedWeapon = w;
                found = true;
                break;
            }
        }  

        if(!found)
        {
            Debug.Log("No weapon found on vehicle");
        }
    }

    public BaseWeapon SelectWeapon(Weapon weaponChoice)
    {
        switch (weaponChoice)
        {
            case Weapon.front:
                selectedWeapon = frontWeapon;
                break;
                
            case Weapon.rear:
                selectedWeapon = rearWeapon;
                break;

            case Weapon.left:
                selectedWeapon = leftWeapon;
                break;

            case Weapon.right:
                selectedWeapon = rightWeapon;
                break;
            
            default:
                return null;
        }
        
        return selectedWeapon;
    }


    public void Fire()
    {   
        selectedWeapon.StartFiring();
    }

    public void StopFiring()
    {
        selectedWeapon.StopFiring();
    }
}
