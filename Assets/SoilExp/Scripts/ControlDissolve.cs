using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlDissolve : MonoBehaviour
{
    public GameObject Root;
    public float DissolveTime = 1.5f;
    private SkinnedMeshRenderer[] _renderers;
    private Material myMaterial;
    void Start()
    {
        // _renderers = GetComponents<SkinnedMeshRenderer>();
        myMaterial = GetComponent<Renderer>().material;
        Debug.Log("myMaterial name " + myMaterial.name);
    }
    
    void Update()
    {
        
    }

    public void StartDisSolve()
    {
        StartCoroutine(Dissolve());
    }

    private IEnumerator Dissolve()
    {
        SetDissloveRate(0);
        float time = 0f;
        while (time < DissolveTime)
        {
            time += Time.deltaTime;
            SetDissloveRate(time/DissolveTime);
            yield return null;
        }
    }

    private void SetDissloveRate(float value)
    {
        int shaderId = Shader.PropertyToID("_ClipRate");
        myMaterial.SetFloat(shaderId,value);
        // foreach (SkinnedMeshRenderer meshRenderer in _renderers)
        // {
        //     Debug.Log("SetDissloveRate material" + meshRenderer.name);
        //     foreach (Material material in meshRenderer.materials)
        //     {
        //         material.SetFloat(shaderId,value);
        //     }
        // }
    }
}
