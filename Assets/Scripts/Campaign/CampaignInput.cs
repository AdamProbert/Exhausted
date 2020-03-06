using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CampaignInput : MonoBehaviour
{
    [SerializeField] private AgentLocomotion agent;
    public InputMaster controls;

    [Header("Hover interactions")]
    [SerializeField] LayerMask interactableLayers;
    Ray hoverRay;
    RaycastHit hoverHit;
    GameObject currentHoverObject;

    [Header("Debug")]
    [SerializeField] bool debug = false;


    private void Awake() 
    {
        controls = new InputMaster();
        controls.Campaign.Selection.performed += ctx => HandleSelectionInput();
        controls.Campaign.CameraZoom.performed += ctx => HandleZoomInput();
    }

    private void Update() 
    {
        HandleHoverInteractions();
    }

    void HandleHoverInteractions()
    {
        hoverRay = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if(Physics.Raycast(hoverRay, out hoverHit, 1000f, interactableLayers))
        {
            if(debug)
            {
                    Debug.DrawLine(Camera.main.transform.position, hoverHit.point, Color.red, 1f);
                    Debug.Log("We hovered over: " + hoverHit.collider.name);
            }

            if(currentHoverObject == null)
            {
                currentHoverObject = hoverHit.collider.gameObject;
                CampaignEventManager.TriggerEvent("TriggerHover", hoverHit.collider.gameObject);
            }

            if(currentHoverObject == hoverHit.collider.gameObject)
            {
                return;
            }

            CampaignEventManager.TriggerEvent("TriggerHover", hoverHit.collider.gameObject);                
        }

        // Stopped hovering
        if(currentHoverObject != null)
        {
            currentHoverObject = null;
            CampaignEventManager.TriggerEvent("TriggerHover", null);
        }
    }

    void HandleSelectionInput()
    {
        Vector3 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit hit;
        // ignoring interactables
        if(CampaignStateMachine.instance.state == CampaignStateMachine.CampaignState.DEFAULT)
        {
            if(Physics.Raycast(ray, out hit, 1000f, ~interactableLayers))
            {
                agent.SetAgentDestination(hit.point);    
            }
        }   
    }

    void HandleZoomInput()
    {
        if(CampaignStateMachine.instance.state == CampaignStateMachine.CampaignState.LANDMARK)
        {
            CampaignEventManager.TriggerEvent("CancelLandmark", null);
        }
    }

    private void OnEnable()
    {
        controls.Campaign.Enable();
    }
 
    private void OnDisable()
    {
        controls.Campaign.Disable();
    }
}
