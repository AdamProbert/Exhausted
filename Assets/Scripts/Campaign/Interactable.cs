using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Interactable : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] GameObject UI;
    [SerializeField] float canvasScaleMultiplier;
    [SerializeField] Animator canvasAnim;

    [Header("Cameras")]
    Transform cam;
    [SerializeField] GameObject virtualCamera;

    bool isActive = false;


    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(UI.activeInHierarchy)
        {   
            // Rotation
            UI.transform.LookAt(2 * UI.transform.position - cam.position);

            //Scale
            float scaleValue = Vector3.Distance(cam.position, UI.transform.position) * canvasScaleMultiplier;
            scaleValue = Mathf.Clamp(scaleValue, 0.1f, 5f);
            UI.transform.localScale = new Vector3(scaleValue, scaleValue, scaleValue);
        }
    }

    void TriggerHoverEventHandlder(GameObject go)
    {
        if(!isActive)
        {
            if(go == this.gameObject)
            {
                Debug.Log("Trigger hover called");
                canvasAnim.SetBool("active", true);
            }
            else if(canvasAnim.GetBool("active") == true)
            {
                Debug.Log("Player stopped hovering");
                canvasAnim.SetBool("active", false);
            }
        }
        
    }

    void PlayerEnteredInteractableRegion()
    {
        if(!isActive)
        {
            isActive = true;
            CampaignStateMachine.instance.SetState(CampaignStateMachine.CampaignState.LANDMARK);
            if(virtualCamera != null)
            {
                virtualCamera.SetActive(true);
            }
            canvasAnim.SetBool("active", true);           
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        Debug.Log("Trigger called");
        // Player entered region
        if(other.gameObject.layer == 8)
        {
            PlayerEnteredInteractableRegion();
        }
    }

    void CancelInteraction(GameObject notused)
    {
        if(isActive)
        {
            isActive = false;
            CampaignStateMachine.instance.SetState(CampaignStateMachine.CampaignState.DEFAULT);
            if(virtualCamera != null)
            {
                virtualCamera.SetActive(false);
            }
            canvasAnim.SetBool("active", false);
        }
    }

    private void OnEnable()
    {
        CampaignEventManager.StartListening("TriggerHover", TriggerHoverEventHandlder);
        CampaignEventManager.StartListening("CancelInteractable", CancelInteraction);
    }
 
    private void OnDisable()
    {
        CampaignEventManager.StopListening("TriggerHover", TriggerHoverEventHandlder);
        CampaignEventManager.StopListening("CancelInteractable", CancelInteraction);
    }
}
