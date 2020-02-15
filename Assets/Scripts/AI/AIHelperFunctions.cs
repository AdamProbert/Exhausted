using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AIHelperFunctions
{
    public static bool CanSeeTarget(Transform source, Transform target, float maxDistance, float awarnessDistance, float viewAngle)
    {
        RaycastHit hit;
        if ( Vector3.Distance(source.position, target.position) > maxDistance ) {
            return false;
        }

        if ( Physics.Linecast(source.position, target.position, out hit) ) {
            if ( hit.collider != target.GetComponent<Collider>() ) {
                return false;
            }
        }

        if ( Vector3.Angle(target.position - source.position, source.forward) > viewAngle) {
            return false;
        }

        if ( Vector3.Distance(source.position, target.position) > awarnessDistance) {
            return false;
        }
        
        return true;
    }
}
