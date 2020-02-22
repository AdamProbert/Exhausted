using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAttachPoint : MonoBehaviour
{

    [SerializeField] float ogScaleModifier;
    [SerializeField] float highlightScaleModifier;
    private Renderer rend;
    
    public void highlight()
    {
        rend.transform.localScale = Vector3.one;
        rend.transform.localScale *= highlightScaleModifier;
    }

    public void removeHighlight()
    {
        rend.transform.localScale = Vector3.one;
        rend.transform.localScale *= ogScaleModifier;
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
