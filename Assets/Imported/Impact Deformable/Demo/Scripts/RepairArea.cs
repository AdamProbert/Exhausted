using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

// Repair area on car derby demoscene
public class RepairArea : MonoBehaviour 
{
    public ImpactDeformable PlayerCallHull;
    public Text Text;

    float repairTime;

    void Awake()
    {
        InvokeRepeating("Repair", 0, 0.1f);
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject == PlayerCallHull.gameObject)
            repairTime = Time.time;
    }

    void Repair()
    {
        bool inRepair = Time.time - repairTime <= 0.11f;

        Text.gameObject.SetActive(inRepair);
        if (inRepair)
            PlayerCallHull.Repair(0.1f);
    }
}
