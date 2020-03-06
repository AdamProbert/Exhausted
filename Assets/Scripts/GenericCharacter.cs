using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericCharacter : MonoBehaviour
{
    [SerializeField] enum animChoice
    {
        Sitting,
        StandingArguing
    }

    [SerializeField] animChoice choice;

    private Dictionary<animChoice, string> animDictionary;
    private Animator animator;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        animDictionary = new Dictionary<animChoice, string> {
            {animChoice.Sitting, "sitting"},
            {animChoice.StandingArguing, "standing_arguing"}
        };

        animator.SetFloat("rando_offset", Random.Range(0f, 3f));
        animator.SetBool(animDictionary[choice], true);
    }
}
