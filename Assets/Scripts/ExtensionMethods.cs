using DocumentFormat.OpenXml.Vml.Spreadsheet;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows;

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
                Pipe hitPipe = collider.GetComponent<Pipe>();

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

    public static string Debug(this IEnumerable array)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        foreach (var item in array)
        {
            if (item == null)
            {
                sb.AppendLine("Item is null");
                continue;
            }

            var nameProperty = item.GetType().GetProperty("name");
            if (nameProperty != null)
            {
                string name = nameProperty.GetValue(item)?.ToString() ?? "null";
                sb.AppendLine(name);
            }
            else
            {
                sb.AppendLine(item.ToString());
            }
        }
        return sb.ToString();
    }
    public static string Base64Encode(this string plainText)
    {
        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
        return System.Convert.ToBase64String(plainTextBytes);
    }

    public static string Base64Decode(this string base64EncodedData)
    {
        var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
        return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
    }

    public static string RemoveWhiteSpace(this string str)
    {
        return string.Concat(str.Where(c => !char.IsWhiteSpace(c)));
    }
}
