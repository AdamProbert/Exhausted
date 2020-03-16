using UnityEngine;
using System.Collections;
 
public class DayNightController : MonoBehaviour {
 
    public Light sun;
    public float secondsInFullDay = 120f;

    [Range(0,1)]
    public float currentTimeOfDay = 0;

    [Header("Status")]
    [SerializeField] float sunset = 0.8f;
    [SerializeField] float sunrise = 0.25f;
    [SerializeField] bool daytime;


    [HideInInspector]
    public float timeMultiplier = 1f;
    
    float sunInitialIntensity;

    bool cycleEnabled = false;
    
    void Start() {
        sunInitialIntensity = sun.intensity;
        UpdateSun(); // Setup sun first time
    }
    
    void Update() {
        if(cycleEnabled)
        {
            UpdateSun();
            UpdateStatus();
            currentTimeOfDay += (Time.deltaTime / secondsInFullDay) * timeMultiplier;
            if (currentTimeOfDay >= 1) {
                currentTimeOfDay = 0;
            }
        }
    }
    
    void UpdateStatus()
    {
        if(currentTimeOfDay > sunrise & !daytime)
        {
            daytime = true;
            CampaignEventManager.TriggerEvent("Sunrise", null);
        }

        if(currentTimeOfDay > sunset & daytime)
        {
            daytime = false;
            CampaignEventManager.TriggerEvent("Sunset", null);
        }
    }
    void UpdateSun() {
        sun.transform.localRotation = Quaternion.Euler((currentTimeOfDay * 360f) - 90, 90, 0);
 
        float intensityMultiplier = 1;
        if (currentTimeOfDay <= 0.23f || currentTimeOfDay >= 0.75f) {
            intensityMultiplier = 0;
        }
        else if (currentTimeOfDay <= 0.25f) {
            intensityMultiplier = Mathf.Clamp01((currentTimeOfDay - 0.23f) * (1 / 0.02f));
        }
        else if (currentTimeOfDay >= 0.73f) {
            intensityMultiplier = Mathf.Clamp01(1 - ((currentTimeOfDay - 0.73f) * (1 / 0.02f)));
        }
 
        sun.intensity = sunInitialIntensity * intensityMultiplier;
    }

    void EnableCycle(Object notused)
    {
        cycleEnabled = true;
    }

    void DisableCycle(Object notused)
    {
        cycleEnabled = false;
    }

    private void OnEnable()
    {
        CampaignEventManager.StartListening("PlayerStartedMoving", EnableCycle);
        CampaignEventManager.StartListening("PlayerReachedDestination", DisableCycle);
    }
 
    private void OnDisable()
    {
        CampaignEventManager.StopListening("PlayerStartedMoving", EnableCycle);
        CampaignEventManager.StopListening("PlayerReachedDestination", DisableCycle);
    }
}