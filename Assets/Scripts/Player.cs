using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public MeshRenderer mr;
    public Texture currTexture;
    private Material material;

    void Start()
    {
        material = mr.material;
    }

    public void SetTex()
    {
        material.SetTexture("_MainTex", currTexture);
    }
}
