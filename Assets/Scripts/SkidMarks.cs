// taken from https://www.youtube.com/watch?time_continue=77&v=iCeNeKOsC2I&feature=emb_logo
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkidMarks : MonoBehaviour
{

    WheelCollider wheel; // TODO: Set as required
    [SerializeField] GameObject skidMarkPrefab;
    GameObject skidMark;
    // Start is called before the first frame update
    void Start()
    {
        skidMark = Instantiate(skidMarkPrefab);
        skidMark.gameObject.SetActive(true);

        wheel = GetComponent<WheelCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        WheelHit groundHit;
        wheel.GetGroundHit(out groundHit);
        if ( Mathf.Abs( groundHit.sidewaysSlip ) > .8 )
		{
		    skidMark.gameObject.SetActive(true);
		}
	    else if ( Mathf.Abs( groundHit.sidewaysSlip ) <= .75 )
		{
		    skidMark.gameObject.SetActive(false);
		}
    }
}
