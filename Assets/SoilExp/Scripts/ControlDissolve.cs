using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlDissolve : MonoBehaviour
{
    public GameObject Root;
    float DissolveTime = 1f;
    // private SkinnedMeshRenderer[] _renderers;
    private Renderer[] _renderers;
    private Material myMaterial;
    private List<Material> myMaterials;
    
    void Start()
    {
        // _renderers = GetComponents<SkinnedMeshRenderer>();
        // myMaterial = GetComponent<Renderer>().material;

        _renderers = GetComponentsInChildren<Renderer>();
        myMaterials = new List<Material>();
        foreach (var renderer in _renderers)
        {
            foreach (var material in renderer.materials)
            {
                myMaterials.Add(material);
            }
        }
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
        foreach (var myMaterial in myMaterials)
        {
            myMaterial.SetFloat(shaderId,value);
        }
        // myMaterial.SetFloat(shaderId,value);
        // foreach (SkinnedMeshRenderer meshRenderer in _renderers)
        // {
        //     Debug.Log("SetDissloveRate material" + meshRenderer.name);
        //     foreach (Material material in meshRenderer.materials)
        //     {
        //         material.SetFloat(shaderId,value);
        //     }
        // }
    }

    public void BackNormal()
    {
        StartCoroutine(Display());
    }
    
    private IEnumerator Display()
    {
        SetDissloveRate(1);
        float time = 0f;
        while (time < DissolveTime)
        {
            time += Time.deltaTime;
            SetDissloveRate((1 - time) / DissolveTime);
            yield return null;
        }
    }

    public void SetActive()
    {
        SetDissloveRate(0);
    }
}