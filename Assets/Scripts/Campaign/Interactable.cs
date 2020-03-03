using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] GameObject UI;
    [SerializeField] float canvasScaleMultiplier;
    [SerializeField] Animator canvasAnim;

    Transform cam;

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
            scaleValue = Mathf.Clamp(scaleValue, 0.5f, 5f);
            UI.transform.localScale = new Vector3(scaleValue, scaleValue, scaleValue);
        }
    }

    void TriggerHoverEventHandlder(GameObject go)
    {
        if(go != null)
        {
            Debug.Log("Trigger hover called with gameobject: " + go.name);
            canvasAnim.SetBool("active", true);
        }
        else
        {
            Debug.Log("Player stopped hovering");
            canvasAnim.SetBool("active", false);
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
