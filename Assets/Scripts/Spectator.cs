using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Spectator : MonoBehaviour
{

    [SerializeField] private List<GameObject> skinList = new List<GameObject>();
    private Animator animator;

    // Start is called before the first frame update
    void Awake()
    {   
        AssignSkin();
    }

    void AssignSkin()
    {
        skinList[Random.Range(0, skinList.Count)].SetActive(true);        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
