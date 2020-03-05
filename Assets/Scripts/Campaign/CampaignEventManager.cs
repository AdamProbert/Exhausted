using System.Collections.Generic;
using UnityEngine;
using System;
 
 
public class CampaignEventManager : MonoBehaviour
{
 
    private Dictionary<string, Action<GameObject>> eventDictionary;
 
    private static CampaignEventManager eventManager;
 
    public static CampaignEventManager instance
    {
        get
        {
            if (!eventManager)
            {
                eventManager = FindObjectOfType(typeof(CampaignEventManager)) as CampaignEventManager;
                if (!eventManager)
                {
                    Debug.LogError("There needs to be one active EventManger script on a GameObject in your scene.");
                }
 
                else
                {
                    eventManager.Init();
                }
            }
            return eventManager;
        }
    }
 
    void Init()
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<string, Action<GameObject>>();
        }
    } 
 
    public static void StartListening(string eventName, Action<GameObject> listener)
    {
 
        Action<GameObject> thisEvent;
 
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent += listener;
            instance.eventDictionary[eventName] = thisEvent;
        }
 
        else
        {
            thisEvent += listener;
            instance.eventDictionary.Add(eventName, thisEvent);
        }
    }
 
    public static void StopListening(string eventName, Action<GameObject> listener)
    {
        if (eventManager == null) return;
 
        Action<GameObject> thisEvent;
 
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent -= listener;
        }
    }
 
    public static void TriggerEvent(string eventName, GameObject go)
    {
        Action<GameObject> thisEvent = null;

        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(go);
        }
    }
}