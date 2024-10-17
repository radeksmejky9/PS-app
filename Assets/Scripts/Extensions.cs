using DocumentFormat.OpenXml.Presentation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static GameObject GetClosestSegment(this GameObject source, GameObject[] segments)
    {
        float sphereRadius = 0.001f;
        float maxRadius = 1.0f;
        float radiusIncrement = 0.001f;

        Vector3 sourcePosition = source.transform.position;

        GameObject closestObject = null;
        float closestDistance = Mathf.Infinity;

        while (closestObject == null && sphereRadius <= maxRadius)
        {

            Collider[] hitColliders = Physics.OverlapSphere(sourcePosition, sphereRadius);

            foreach (Collider collider in hitColliders)
            {
                GameObject hitObject = collider.gameObject;

                if (System.Array.Exists(segments, segment => segment == hitObject))
                {
                    Vector3 directionToTarget = hitObject.transform.position - sourcePosition;
                    float dSqrToTarget = directionToTarget.sqrMagnitude;
                    //float distance = Vector3.Distance(source.transform.position, hitObject.transform.position);

                    if (dSqrToTarget < closestDistance)
                    {
                        closestDistance = dSqrToTarget;
                        closestObject = hitObject;
                    }
                }
            }

            if (closestObject == null)
            {
                sphereRadius += radiusIncrement;
            }
        }


        /*var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = source.transform.position;
        sphere.transform.localScale = new Vector3(sphereRadius * 2, sphereRadius * 2, sphereRadius * 2);*/

        return closestObject;
    }

    public static Vector2 Abs(this Vector2 _vector)
    {
        float _x = Mathf.Abs(_vector.x);
        float _y = Mathf.Abs(_vector.y);

        return new Vector2(_x, _y);
    }

    public static Vector3 Abs(this Vector3 _vector)
    {
        float _x = Mathf.Abs(_vector.x);
        float _y = Mathf.Abs(_vector.y);
        float _z = Mathf.Abs(_vector.y);

        return new Vector3(_x, _y, _z);
    }
}
