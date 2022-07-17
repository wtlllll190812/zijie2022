using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Images : MonoBehaviour
{
    private Material mat;

    void Awake()
    {
        mat=GetComponent<MeshRenderer>().material;
    }
    void OnMouseDown()
    {
        mat.SetFloat("_currentTex",1.0f);
    }
}
