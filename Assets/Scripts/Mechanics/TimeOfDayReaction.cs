using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeOfDayReaction : MonoBehaviour
{
    [SerializeField] List<GameObject> reactors = new List<GameObject>();
    [SerializeField] bool autoFindLights = true;

    private void Start() 
    {
        if(autoFindLights)
        {
            foreach (Light l in transform.GetComponentsInChildren<Light>())
            {
                reactors.Add(l.gameObject);
            }
        }
    }
    void Sunrise(GameObject notused)
    {
        foreach (GameObject item in reactors)
        {
            item.SetActive(false);
        }
    }

    void Sunset(GameObject notused)
    {
        foreach (GameObject item in reactors)
        {
            item.SetActive(true);
        }
    }

    private void OnEnable()
    {
        CampaignEventManager.StartListening("Sunrise", Sunrise);
        CampaignEventManager.StartListening("Sunset", Sunset);
    }
 
    private void OnDisable()
    {
        CampaignEventManager.StopListening("Sunset", Sunset);
        CampaignEventManager.StopListening("Sunrise", Sunrise);
    }
}
