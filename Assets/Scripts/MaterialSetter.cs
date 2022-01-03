using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(MeshRenderer))]

public class MaterialSetter : MonoBehaviour
{

    [SerializeField] private MeshRenderer _meshRenderer;

    public MeshRenderer meshRenderer
    {
        get
        {
            if(_meshRenderer == null)          
                _meshRenderer = GetComponent<MeshRenderer>();
                return _meshRenderer;       
        }
    }

    public void SetSingleMaterial(Material material)
    {
        meshRenderer.material = material;
    }

}
