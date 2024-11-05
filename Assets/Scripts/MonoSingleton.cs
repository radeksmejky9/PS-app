using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindExistingInstance() ?? CreateNewInstance();
            }
            return _instance;
        }
    }
    private static T FindExistingInstance()
    {
        T[] existingInstances = FindObjectsOfType<T>();

        if (existingInstances == null || existingInstances.Length == 0) return null;

        return existingInstances[0];
    }

    private static T CreateNewInstance()
    {
        var containerGO = new GameObject("__" + typeof(T).Name + " (Singleton)");
        return containerGO.AddComponent<T>();
    }
}
