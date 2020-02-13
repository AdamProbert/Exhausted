using UnityEngine;
using System.Collections;

// AI control over a Car
public class AIControl : CarControl 
{
    void Start()
    {
        StartCoroutine(ChangeIdea());
    }

    IEnumerator ChangeIdea()
    {
        while (enabled)
        {
            // Change car control settings on random time intervals
            // Enough for demonstration purposes
            ControlCar(Random.Range(0, 3) - 1, Random.Range(0, 3) - 1);
            yield return new WaitForSeconds(Random.value * 3);
        }        
    }
}
