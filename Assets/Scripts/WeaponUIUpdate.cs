using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class WeaponUIUpdate : MonoBehaviour
{
    [SerializeField] List<GameObject> weapons = new List<GameObject>();
    [SerializeField] GameObject iconPrefab;

    [SerializeField] WeaponController weaponController;

    public InputMaster controls; // Our custom InputSystem control scheme

    // Start is called before the first frame update
    void Start()
    {

    }

    public void RegisterWeapons(List<BaseWeapon> weapons)
    {
        foreach(BaseWeapon weapon in weapons)
        {
            GameObject icon = Instantiate(iconPrefab, transform.position, Quaternion.identity, transform);
            Image image = icon.GetComponentInChildren<Image>();
            image.sprite = weapon.GetUIImage();
            this.weapons.Add(icon);
            icon.GetComponent<Animator>().Play("WeaponRegister", 0, 0f);
        }
    }

    public void SetCurrentWeapon(int weaponNumber)
    {
        Debug.Log("WeaponUI Update: Set current weapon called");
        weapons[weaponNumber-1].GetComponent<Animator>().Play("WeaponSelect", 0, 0f);
    }
}
