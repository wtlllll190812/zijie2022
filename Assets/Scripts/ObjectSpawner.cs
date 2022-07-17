using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject objPref;
    public List<ImageData> objNameList=new List<ImageData>();
    public int distance;
    public float interval;

    public IEnumerator GenObj()
    {
        while(true)
        {
            Spawn(Random.Range(0,objNameList.Count),transform);
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

    public void Spawn(int index, Transform tr)
    {
        var newObj = Instantiate(objPref, tr);
        newObj.GetComponent<MeshRenderer>().material.SetTexture("_Tex1", objTexList1[index]);
        newObj.GetComponent<MeshRenderer>().material.SetTexture("_Tex2", objTexList2[index]);
    }
}
public struct ImageData
{ 
    public string name;
    public Texture tex1;
    public Texture tex2;
}
