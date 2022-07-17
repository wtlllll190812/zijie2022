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
    void Start()
    {
        StartCoroutine(GenObj());
    }
    public IEnumerator GenObj()
    {
        while(true)
        {
            int verIndex = Random.Range(0, curve.vertices.Length);
            verIndex -= verIndex % distance;
            if(!point.Contains(verIndex))
            {
                point.Add(verIndex);
                int texindex = Random.Range(0, objList.Count);
                Quaternion q = Quaternion.identity;
                q.SetLookRotation(-curve.meshFilter.transform.TransformDirection(curve.tangents[verIndex]), curve.meshFilter.transform.TransformDirection(curve.normals[verIndex]));
                Spawn(Random.Range(0, texindex), curve.vertices[verIndex], q);
            }
            yield return new WaitForSeconds(interval);
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

    public void Spawn(int index, Vector3 pos,Quaternion rat)
    {
        var newObj = Instantiate(objPref, pos, rat);
        newObj.transform.Translate(Vector3.right*Random.Range(-offset,offset),Space.Self);
        var obj = newObj.transform.GetChild(0);
        Material mat = obj.GetComponent<MeshRenderer>().material;
        mat.SetTexture("_Tex1", objList[index].tex1);
        mat.SetTexture("_Tex2", objList[index].tex2);
        obj.GetComponent<Images>().imageName = objList[index].name;
        newObj.transform.SetParent(transform);
    }
}
[System.Serializable]
public struct ImageData
{ 
    public string name;
    public Texture tex1;
    public Texture tex2;
}
