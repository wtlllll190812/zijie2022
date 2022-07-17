using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject objPref;
    public int distance;
    public float interval;
    public float offset;
    public List<ImageData> objList=new List<ImageData>();
    public HashSet<int> point=new HashSet<int>();
    public Curve curve;

    private List<GameObject> objs=new List<GameObject>();
    void Start()
    {
        StartCoroutine(GenObj());
    }

    public IEnumerator GenObj()
    {
        while(!GameManager.instance.gameStarted)
            yield return null;
        for (int i = 0; i < 10; i++)
        {
            Spawn();
        }
        yield return new WaitForSeconds(interval);
        while (true)
        {
            Spawn();
            yield return new WaitForSeconds(interval);
        }
    }

    public void Clear()
    {
        for (int i = 0; i < objs.Count; i++)
        {
            Destroy(objs[i]);
        }
    }
    //public void Spawn(string name,Transform tr)
    //{
    //    int index = objNameList.IndexOf(name);
    //    if (index != -1)
    //    {
    //        var newObj=Instantiate(objPref, tr);
    //        newObj.GetComponent<MeshRenderer>().material.SetTexture("_Tex1",objTexList1[index]);
    //        newObj.GetComponent<MeshRenderer>().material.SetTexture("_Tex2",objTexList2[index]);
    //    }
    //}

    public void Spawn()
    {
        int verIndex = Random.Range(0, curve.vertices.Length);
        verIndex -= verIndex % distance;
        if (!point.Contains(verIndex))
        {
            point.Add(verIndex);
            int texindex = Random.Range(0, objList.Count);
            Quaternion quat = Quaternion.identity;
            quat.SetLookRotation(-curve.meshFilter.transform.TransformDirection(curve.tangents[verIndex]), curve.meshFilter.transform.TransformDirection(curve.normals[verIndex]));

            int index = Random.Range(0, texindex);
            var newObj = Instantiate(objPref, curve.vertices[verIndex], quat);
            newObj.transform.Translate(Vector3.right * Random.Range(-offset, offset), Space.Self);
            var obj = newObj.transform.GetChild(0);
            Material mat = obj.GetComponent<MeshRenderer>().material;
            mat.SetTexture("_Tex1", objList[index].tex1);
            mat.SetTexture("_Tex2", objList[index].tex2);
            obj.GetComponent<Images>().imageName = objList[index].name;
            newObj.transform.SetParent(transform);

            objs.Add(newObj);
        }
    }
}
[System.Serializable]
public struct ImageData
{ 
    public string name;
    public Texture tex1;
    public Texture tex2;
}
