using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Curve : MonoBehaviour
{
    public MeshFilter meshFilter;
    public Transform pos;
    public bool canMove=true;
    public Vector3[] normals;
    public Vector3[] vertices;
    void Start()
    {
        cal();
        StartCoroutine(Move());
    }

    private void cal()
    {
        vertices = meshFilter.sharedMesh.vertices;
        normals = meshFilter.sharedMesh.normals;
        var meshTr = meshFilter.transform;
        //计算向量
        int[] triangles = meshFilter.sharedMesh.triangles;
        int triangelCount = triangles.Length / 3;
        for (int i = 0; i < triangelCount; i++)
        {
            int index = i * 3;

            Vector3 vertext_start = meshTr.TransformPoint(vertices[triangles[index]]);
            Vector3 vertext_middle = meshTr.TransformPoint(vertices[triangles[index + 1]]);
            Vector3 vertext_end = meshTr.TransformPoint(vertices[triangles[index + 2]]);

            Vector3 startToMiddle = vertext_middle - vertext_start;
            Vector3 middleToEnd = vertext_end - vertext_middle;

            Vector3 triangleCenter = Vector3.zero;
            triangleCenter.x = (vertext_start.x + vertext_middle.x + vertext_end.x) / 3.0f;
            triangleCenter.z = (vertext_start.z + vertext_middle.z + vertext_end.z) / 3.0f;
            triangleCenter.y = (vertext_start.y + vertext_middle.y + vertext_end.y) / 3.0f;

            Vector3 normal = Vector3.Cross(startToMiddle, middleToEnd).normalized;

            normals[triangles[index]] = normal;
            normals[triangles[index + 1]] = normal;
            normals[triangles[index + 2]] = normal;
            vertices[triangles[index]] = vertext_start;
            vertices[triangles[index + 1]] = vertext_middle;
            vertices[triangles[index + 2]] = vertext_end;
        }
    }
    
    private IEnumerator Move()
    {
        int count = vertices.Length -1;
        while (canMove)
        {
            if(count<=0)
                count = vertices.Length - 1;
            //transform.position = vertices[count];
            //transform.LookAt(pos.position, normals[count]);
            Debug.DrawRay(vertices[count], normals[count]);
            //Debug.Log(normals[count]);
            count -=2;
            //yield return null;// new WaitForSeconds(0.1f);
        }
        yield return null;
    }
    //void OnDrawGizmos()
    //{
    //    if (!caled)
    //    {
    //        caled = true;
    //        cal();
    //    }
    //    Gizmos.color = Color.yellow;
    //    for (int i = 0; i < vertices.Length; i += 2)
    //    {
    //        Gizmos.DrawRay(vertices[i], normals[i]);
    //    }
    //}
}
