using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static GameObject GetClosestSegment(this GameObject source, GameObject[] segments)
    {
        GameObject bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = source.transform.position;
        foreach (GameObject potentialTarget in segments)
        {
            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }

        return bestTarget;
    }
}
