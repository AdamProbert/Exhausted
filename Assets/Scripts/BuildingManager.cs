using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    private  InputMaster controls; // Our custom InputSystem control scheme
    [SerializeField] List<GameObject> vehicles = new List<GameObject>();
    [SerializeField] List<GameObject> attachments = new List<GameObject>();
    [SerializeField] float rotationSpeed;
    private GameObject currentVehicle;
    private int currentVehicleIndex = 0;
    Player player;

    List<CarAttachPoint> attachPoints = new List<CarAttachPoint>();
    private int currentAttachPointIndex = 0;

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
        player.GetComponent<UserInput>().enabled = false;
        controls.UI.Click.performed += ctx => HandleClick();
        controls.UI.CycleAttachPoint.performed += ctx => CycleAttachPoint(ctx.ReadValue<float>());
        SwapVehicle();
    }

    void FixedUpdate() 
    {
        rotateInput = controls.UI.Rotate.ReadValue<float>();
        if(rotateInput != 0)
        {
            RotateVehicle();
        }

        // cycleAttachPointInput = controls.UI.CycleAttachPoint.ReadValue<float>();
        // Debug.Log("Attach point input: " + cycleAttachPointInput);
        // if(cycleAttachPointInput != 0)
        // {
        //     HighlightAttachPoint(false);
        //     CycleAttachPoint();
        //     HighlightAttachPoint(true);
        // }        
    }

    public void SelectWeapon()
    {
        
    }

    private void HighlightAttachPoint(bool highlight)
    {
        if(highlight)
        {
            attachPoints[currentAttachPointIndex].highlight();    
        }
        else
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
        }
        
        currentVehicle = Instantiate(vehicles[currentVehicleIndex], player.transform.position, player.transform.rotation, player.transform);
        attachPoints.AddRange(currentVehicle.GetComponentsInChildren<CarAttachPoint>());
        Debug.Log("New attach points: " + attachPoints);
    }

    private void HandleClick()
    {
        Debug.Log("Building manager: Handle click called");
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
