using System;
using UnityEngine;

public class SingletonMonoBehaviour<T> : MonoBehaviour where T : class
{
    public static T Instance;

    protected virtual void Awake()
    {
        if (Instance == null)
            Instance = this as T;
        else
            Destroy(this);
    }
}
