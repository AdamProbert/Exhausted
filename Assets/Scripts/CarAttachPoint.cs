using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAttachPoint : MonoBehaviour
{

    [SerializeField] float ogScaleModifier;
    [SerializeField] float highlightScaleModifier;
    private Renderer rend;
    public Attachment currentAttachment;
    public bool hasAttachment = false;
    

    public void Attach(Attachment attachment)
    {
        hasAttachment = true;
        currentAttachment = attachment;
        if(transform.root.GetComponent<WeaponController>())
        {
            transform.root.GetComponent<WeaponController>().AddWeapon(attachment.GetComponent<BaseWeapon>());
        }   
    }

    public void Detach()
    {
        transform.root.GetComponent<WeaponController>().RemoveWeapon(currentAttachment.GetComponent<BaseWeapon>());
        Destroy(currentAttachment.gameObject);
        hasAttachment = false;
        currentAttachment = null;
    }


    public void highlight()
    {
        rend.transform.localScale = Vector3.one;
        rend.transform.localScale *= highlightScaleModifier;
        rend.material.SetFloat("_Highlighted", 1);
    }

    public void removeHighlight()
    {
        rend.transform.localScale = Vector3.one;
        rend.transform.localScale *= ogScaleModifier;
        rend.material.SetFloat("_Highlighted", 0);
    }

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponentInChildren<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
