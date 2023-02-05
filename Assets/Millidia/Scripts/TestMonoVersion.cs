using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

public class TestMonoVersion : MonoBehaviour
{
    [ContextMenu("Show Version")]
    public void MonoVersion()
    {
        Debug.Log(Application.unityVersion);
        
        Type type = Type.GetType("Mono.Runtime");
        if (type != null)
        {
            MethodInfo displayName = type.GetMethod("GetDisplayName", BindingFlags.NonPublic | BindingFlags.Static);
            if (displayName != null)
                Debug.Log(displayName.Invoke(null, null));

            MethodInfo[] methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            for (int i = 0; i < methods.Length; i++)
            {
                MethodInfo m = methods[i];
                Debug.Log((m.IsPublic ? "public " : (m.IsPrivate ? "private " : "")) + (m.IsStatic ? "static " : " ") + m.ReturnType.Name + " " + m.Name + " " + m.GetParameters().Length);
            }
        }
 
    }

   
}
