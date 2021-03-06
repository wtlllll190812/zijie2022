using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Images : MonoBehaviour
{
    private Material mat;
    public string imageName;
    void Awake()
    {
        mat=GetComponent<MeshRenderer>().material;
    }
    void OnMouseDown()
    {
        StartCoroutine(Display());
    }
    IEnumerator Display()
    {
        float c = mat.GetFloat("_currentTex");
        if (c - 0.1f <= -1)
        {
            GameManager.instance.score++;
            GetComponent<Animator>().enabled = true;
            GetComponent<MeshCollider>().enabled = false;
            GetComponent<AudioSource>().enabled = true;
        }
        while (c<1.0f)
        {
            mat.SetFloat("_currentTex", c);
            c += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
