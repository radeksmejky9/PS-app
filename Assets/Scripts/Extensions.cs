using DocumentFormat.OpenXml.Vml.Spreadsheet;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static Pipe GetClosestPipe(this Transform source, Pipe[] pipes)
    {
        float sphereRadius = 0.001f;
        float maxRadius = 1.0f;
        float radiusIncrement = 0.001f;

        Vector3 sourcePosition = source.position;

        Pipe closestObject = null;
        float closestDistance = Mathf.Infinity;


        Collider[] initialHitColliders = Physics.OverlapSphere(sourcePosition, maxRadius);
        if (initialHitColliders.Length == 0)
        {
            return null;
        }

        while (sphereRadius <= maxRadius)
        {
            Collider[] hitColliders = Physics.OverlapSphere(sourcePosition, sphereRadius);

            foreach (Collider collider in hitColliders)
            {
                Pipe hitPipe = collider.GetComponent<Pipe>(); // Assuming Pipe is a component on the GameObject

                if (hitPipe != null && System.Array.Exists(pipes, pipe => pipe == hitPipe))
                {
                    Vector3 directionToTarget = hitPipe.transform.position - sourcePosition;
                    float dSqrToTarget = directionToTarget.sqrMagnitude;

                    if (dSqrToTarget < closestDistance)
                    {
                        closestDistance = dSqrToTarget;
                        closestObject = hitPipe;
                    }
                }
            }

            if (closestObject != null) return closestObject;
            sphereRadius += radiusIncrement;
        }

        return null;
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
