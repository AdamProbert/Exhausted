using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// Damage UI bar for car in CarDerby demoscene
public class UICarStructure : MonoBehaviour 
{
    public Car Car;

    Image Image;

    void Awake()
    {
        Image = GetComponent<Image>();
    }

    void Update()
    {
        Image.fillAmount = 1 - Car.CarDamage;
    }
}
