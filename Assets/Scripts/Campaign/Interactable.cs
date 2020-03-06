using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] GameObject UI;
    [SerializeField] Animator canvasAnim;

    [Header("Cameras")]
    Transform cam;

    [Header("State")]
    bool hidden = false;
    bool isActive = false;



    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.transform;
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

    private void OnEnable()
    {
        CampaignEventManager.StartListening("TriggerHover", TriggerHoverEventHandlder);
    }
 
    private void OnDisable()
    {
        CampaignEventManager.StopListening("TriggerHover", TriggerHoverEventHandlder);
    }
}