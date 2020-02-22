using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BuildingUIManager))]
public class BuildingManager : MonoBehaviour
{
    private  InputMaster controls; // Our custom InputSystem control scheme
    private BuildingUIManager buildingUIManager;
    [SerializeField] List<GameObject> vehicles = new List<GameObject>();
    [SerializeField] List<Attachment> attachments = new List<Attachment>();
    [SerializeField] float rotationSpeed;

    [SerializeField] ParticleSystem spawnVehicleEffect;
    private GameObject currentVehicle;
    private int currentVehicleIndex = 0;
    Player player;

    List<CarAttachPoint> attachPoints = new List<CarAttachPoint>();
    private int currentAttachPointIndex = -1;

    // Inputs
    float rotateInput = 0;
    float cycleAttachPointInput = 0;

    private void Awake() 
    {
        controls = new InputMaster();
    }

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();    
        controls.UI.CycleAttachPoint.performed += ctx => CycleAttachPoint(ctx.ReadValue<float>());
        controls.UI.CycleVehicle.performed += ctx => CycleVehicle();
        buildingUIManager = GetComponent<BuildingUIManager>();
        SwapVehicle();
        PopulateUI();
        GameManager.Instance.SetGameState(GameManager.GameState.BUILD);
    }

    void FixedUpdate() 
    {
        rotateInput = controls.UI.Rotate.ReadValue<float>();
        if(rotateInput != 0)
        {
            RotateVehicle();
        }
    }

    public void AttachmentSelected(int attachmentIndex)
    {   
        if(currentAttachPointIndex != -1)
        {
            if(attachPoints[currentAttachPointIndex].hasAttachment)
            {
                Debug.Log("Destroying thing");
                attachPoints[currentAttachPointIndex].Detach();
            }

            Attachment newAttachment = Instantiate(
                attachments[attachmentIndex],
                attachPoints[currentAttachPointIndex].transform.position,
                attachPoints[currentAttachPointIndex].transform.rotation,
                player.transform.Find("Weapons")
            );

            attachPoints[currentAttachPointIndex].Attach(newAttachment.GetComponent<Attachment>());
        }
    }

    public void OnPlayClicked()
    {
        // Notifiy manager
        GameManager.Instance.BuildComplete(player);
        foreach(CarAttachPoint cap in attachPoints)
        {
            cap.gameObject.SetActive(false);
        }
    }

    private void PopulateUI()
    {
        buildingUIManager.PopulateAttachments(attachments);
    }

    private void HighlightAttachPoint(bool highlight)
    {
        if(highlight && currentAttachPointIndex != -1)
        {
            attachPoints[currentAttachPointIndex].highlight();    
        }
        else if(!highlight && currentAttachPointIndex != -1)
        {
            attachPoints[currentAttachPointIndex].removeHighlight();    
        }
    }

    private void RotateVehicle()
    {
        player.transform.Rotate( new Vector3(0, rotateInput*rotationSpeed, 0), Space.Self );
    }

    private void SwapVehicle()
    {   
        if(currentVehicle)
        {
            Destroy(currentVehicle);
            attachPoints.Clear();
            currentAttachPointIndex = -1;
            foreach(Transform g in player.transform.Find("Weapons"))
            {
                Destroy(g.gameObject);
            }
        }
        
        currentVehicle = Instantiate(vehicles[currentVehicleIndex], player.transform.position, player.transform.rotation, player.transform);
        ParticleSystem effect = Instantiate(spawnVehicleEffect, player.transform.position, Quaternion.Euler(90, 0, 0));
        Destroy(effect.gameObject, 5f);

        attachPoints.AddRange(currentVehicle.GetComponentsInChildren<CarAttachPoint>());
        Debug.Log("New attach points: " + attachPoints);
    }

    private void CycleVehicle()
    {
        Debug.Log("Building manager: Cycling vehicle");
        IncrementVehicleIndex();
        SwapVehicle();
    }

    private void CycleAttachPoint(float direction)
    {
        HighlightAttachPoint(false);
        Debug.Log("Cycle attach point called: " + direction);
        if(direction > 0)
        {
            currentAttachPointIndex += 1;
            if(currentAttachPointIndex >= attachPoints.Count)
            {
                currentAttachPointIndex = 0;
            }
        }

        else if(direction < 0)
        {
            currentAttachPointIndex -= 1;
            if(currentAttachPointIndex < 0)
            {
                currentAttachPointIndex = attachPoints.Count-1;
            }
        }
        HighlightAttachPoint(true);        
    }

    private void IncrementVehicleIndex()
    {
        currentVehicleIndex += 1;
        if(currentVehicleIndex >= vehicles.Count)
        {
            currentVehicleIndex = 0;
        }
    }

    private void IncrementAttachPointIndex()
    {
        currentAttachPointIndex += 1;
        if(currentAttachPointIndex >= attachPoints.Count)
        {
            currentAttachPointIndex = 0;
        }
    }

    private void OnEnable() 
    {
        controls.UI.Enable();    
    }

    private void OnDisable() 
    {
        controls.UI.Disable();
    }
}
