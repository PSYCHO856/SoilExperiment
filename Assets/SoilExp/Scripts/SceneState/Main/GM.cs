using UnityEngine;
using System.Collections;

public class GM : MonoBehaviour
{
    public CustomArrays[] Arrays;

}
[System.Serializable]
public class CustomArrays
{
    public float[] Array;

    public float this[int index]
    {
        get
        {
            return Array [index];
        }
    }

    public CustomArrays()
    {
        this.Array= new float[4];
    }

    public CustomArrays(int index)
    {
        this.Array= new float[index];
    }

}