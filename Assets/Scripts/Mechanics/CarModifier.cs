using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;


public class CarModifier : MonoBehaviour
{   
    [Header("Boooost")]
    [SerializeField] private ParticleSystem boostFXPrefab;
    [SerializeField] private AudioClip boostSound;
    [SerializeField] public float maxBoost = 100;
    [SerializeField] float boostRestoreSpeed = 0.3f;
    [SerializeField] float boostUsedSpeed = 0.7f;
    public float currentBoost;
    private bool boosting;
    private ParticleSystem boostFX;


    PlayerEventManager playerEventManager;
    CarController carController;
    AudioSource audioSource;

    CarSpecOverride carSpec;

    bool alive = false;

    private void Awake() 
    {
        carController = GetComponent<CarController>();
        audioSource = GetComponent<AudioSource>();
        playerEventManager = GetComponent<PlayerEventManager>();
    }
    private void Start() 
    {
        currentBoost = maxBoost;
    }

    private void OnEnable() 
    {
        playerEventManager.OnPlayerStateChanged += HandlePlayerStateChange;
        playerEventManager.OnVehicleLoad += SetupVehicle;    
    }

    private void OnDisable() 
    {
        playerEventManager.OnPlayerStateChanged -= HandlePlayerStateChange;    
        playerEventManager.OnVehicleLoad -= SetupVehicle;
    }

    private void SetupVehicle()
    {
        carSpec = GetComponentInChildren<CarSpecOverride>();

        GetComponent<Rigidbody>().mass = carSpec.vehicleMass;
        GetComponent<Rigidbody>().centerOfMass = carSpec.centrOfMass;

        carController.m_Topspeed = carSpec.maxSpeed;
        carController.m_MaximumSteerAngle = carSpec.maxSteeringAngle;
        carController.m_CarDriveType = carSpec.driveType;
        carController.m_FullTorqueOverAllWheels = carSpec.torqueOverAllWheels;
        carController.flyingRotationTorque = carSpec.flyingRotationTorque;
    }

    public void HandlePlayerStateChange(Player.state newstate)
    {
        if(newstate == Player.state.Alive)
        {
            alive = true;
        }
    }
    private void Update() 
    {
        if(alive)
        {
            if(currentBoost < maxBoost && !boosting)
            {
                currentBoost += boostRestoreSpeed * Time.deltaTime;
            }

            if(currentBoost > 0 && boosting)
            {
                currentBoost -= boostUsedSpeed * Time.deltaTime;
            }

            if(currentBoost <= 1)
            {
                boosting = false;
                carController.m_Topspeed = carController.OriginalMaxSpeed;
                playerEventManager.OnPlayerBoost(false);
            }
        }
    }

    public void Boost(bool shouldBoost)
    {
        if(shouldBoost && !boosting && currentBoost > 0)
        {
            boosting = true;
            carController.m_Topspeed = carController.OriginalMaxSpeed * 1.5f;
            playerEventManager.OnPlayerBoost(true);    
        }
        else if (!shouldBoost && boosting)
        {
            boosting = false;
            playerEventManager.OnPlayerBoost(false);
            carController.m_Topspeed = carController.OriginalMaxSpeed;
        }
    }
}
