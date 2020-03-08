using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CampaignPlayer : MonoBehaviour
{
    [Header("Resources")]
    [SerializeField] int _fuel;
    [SerializeField] int _caps;
    [SerializeField] int _scrap;

    [SerializeField] GameObject defaultVehicle;
    [SerializeField] GameObject currentVehicle;

    [SerializeField] public int fuel
    {
        get{return _fuel;}
        set
        {
            if(_fuel == value)
                return;

            _fuel = value;
            fuelValue.text = _fuel.ToString();
            if(OnChangeFuel != null)
            {
                OnChangeFuel(_fuel);
            }   
        }
    }

    [SerializeField] public int caps
    {
        get{return _caps;}
        set
        {
            if(_caps == value)
                return;

            _caps = value;
            capsValue.text = _caps.ToString();
            if(OnChangeCaps != null)
            {
                OnChangeCaps(_caps);
            }   
        }
    }

    [SerializeField] public int scrap
    {
        get{return _scrap;}
        set
        {
            if(_scrap == value)
                return;

            _scrap = value;
            scrapValue.text = _scrap.ToString();
            if(OnChangeScrap != null)
            {
                OnChangeScrap(_scrap);
            }   
        }
    }
    
    [Header("UI stuff")]
    [SerializeField] TextMeshProUGUI fuelValue;
    [SerializeField] TextMeshProUGUI capsValue;
    [SerializeField] TextMeshProUGUI scrapValue;

    // Events & Delegates
    public delegate void ChangeFuel(float f);
    public static event ChangeFuel OnChangeFuel;
    public delegate void ChangeCaps(float c);
    public static event ChangeCaps OnChangeCaps;
    public delegate void ChangeScrap(float s);
    public static event ChangeScrap OnChangeScrap;
    
    private void Awake() 
    {
        LoadPlayer();
        LoadVehicle();    
    }
    
    // In/De-crementers
    public void IncreaseFuel(int amount)
    {
        fuel += amount;
    }

    public void DecreaseFuel(int amount)
    {
        fuel -= amount;
        if(fuel < 0)
            fuel = 0;
    }

    public void IncreaseCaps(int amount)
    {
        caps += amount;
    }

    public void DecreaseCaps(int amount)
    {
        caps -= amount;
        if(caps < 0)
            caps = 0;
    }

    public void IncreaseScrap(int amount)
    {
        scrap += amount;
    }

    public void DecreaseScrap(int amount)
    {
        scrap -= amount;
        if(scrap < 0)
            scrap = 0;
    }


    public void SavePlayer()
    {
        SaveSystem.SavePlayer(this);
    }

    public void LoadPlayer()
    {
        CampaignPlayerData data = SaveSystem.LoadPlayer();
        if(data != null)
        {
            caps = data.playerCaps;
            fuel = data.playerFuel;
            scrap = data.playerScrap;

            transform.position = new Vector3(data.position[0], data.position[1], data.position[2]);
        }
    }

    public void LoadVehicle()
    {
        PlayerConfig config = SaveSystem.LoadPlayerConfig();
        if(config.baseCarPrefabName != null)
        {
            currentVehicle = Instantiate(Resources.Load("Cars/" + config.baseCarPrefabName) as GameObject, transform.position, transform.rotation, transform);
            CarAttachPoint[] attachPoints = currentVehicle.GetComponentsInChildren<CarAttachPoint>();

            if(config.weaponPrefabNames != null)
            {
                for (int i = 0; i < config.weaponPrefabNames.Length; i++)
                {
                    if(config.weaponPrefabNames[i] != "NONE")
                    {
                        Attachment w = Instantiate(Resources.Load<GameObject>("CarWeapons/" + config.weaponPrefabNames[i])).GetComponent<Attachment>();

                        w.transform.position = attachPoints[i].transform.position;
                        w.transform.rotation = attachPoints[i].transform.rotation;
                        w.transform.parent = attachPoints[i].transform;
                        attachPoints[i].Attach(w);    
                    }
                }
            }
        }
        // If no saved vehicle - load the default golf cart lol
        else
        {
            Instantiate(defaultVehicle, transform.position, transform.rotation, transform);
        }

        currentVehicle.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }

    public void HandleBattleStarting(GameObject notused)
    {
        SavePlayer();
    }

    private void OnEnable()
    {
        CampaignEventManager.StartListening("BattleStarting", HandleBattleStarting);
    }
 
    private void OnDisable()
    {
        CampaignEventManager.StopListening("BattleStarting", HandleBattleStarting);
    }
}

