using System;
using System.Collections.Generic;
using UnityEngine;

public class SingletonBase<T> where T : class, new()
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new T();
            }
            return instance;
        }
    }
}

public abstract class SingletonBaseComponent<T> : MonoBehaviour where T : Component
{
    private static T instance;
    public static T Instance
    {
        set { instance = value; }
        get
        {
            return instance;
        }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
        }

        OnAwake();
    }

    void Start()
    {
        OnStart();
    }

    protected abstract void OnAwake();

    protected abstract void OnStart();
}

