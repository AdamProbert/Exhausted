using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CampaignInput : MonoBehaviour
{
    [SerializeField] private AgentLocomotion agent;
    public InputMaster controls;

    private void Awake() 
    {
        controls = new InputMaster();
        controls.Campaign.Selection.performed += ctx => HandleSelectionInput();
    }


    void HandleSelectionInput()
    {
        Vector3 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit))
        {
            
            agent.SetAgentDestination(hit.point);
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
