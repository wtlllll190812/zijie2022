using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Curve : MonoBehaviour
{
    public MeshFilter meshFilter;
    public float speed;
    public bool canMove = true;
    public Vector3[] normals;
    public Vector3[] vertices;
    public Vector4[] tangents;

    private Vector3 targetPos;
    private Quaternion targetRot;
    private int path;
    
    void Awake()
    {
        meshFilter.sharedMesh.RecalculateNormals();
        vertices = meshFilter.sharedMesh.vertices;
        normals = meshFilter.sharedMesh.normals;
        tangents = meshFilter.sharedMesh.tangents;
        targetPos = transform.position;
        StartCoroutine(Move());
    }
    
    void FixedUpdate()
    {
        transform.position=Vector3.Lerp(transform.position, targetPos, speed);
        transform.rotation= Quaternion.Lerp(transform.rotation,targetRot,speed);
        Debug.DrawRay(transform.position, transform.forward, Color.red);
        
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            path = -1;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            path = 1;
        }
    }

    private IEnumerator Move()
    {
        int count = vertices.Length - 1;
        while (canMove)
        {
            if (count <= 0)
                count = vertices.Length - 1;
            targetPos = vertices[count];// + transform.InverseTransformVector(transform.right * path/10);
            Quaternion q=Quaternion.identity;
            q.SetLookRotation(-meshFilter.transform.TransformDirection(tangents[count]), meshFilter.transform.TransformDirection(normals[count]));
            targetRot = q;
            Debug.DrawRay(transform.position,transform.forward);
            //transform.LookAt(transform.position + Vector3.Cross(normals[count], transform.right), normals[count]);
            count -= 2;
            yield return new WaitForSeconds(0.2f);
        }
        yield return null;
    }

    private void Calculate()
    {
        vertices = meshFilter.sharedMesh.vertices;
        normals = meshFilter.sharedMesh.normals;
        var meshTr = meshFilter.transform;

        Vector3 vertext_start = Vector3.zero;
        Vector3 vertext_middle = Vector3.zero;
        Vector3 vertext_end;
        for (int i = 0; i < vertices.Length; i++)
        {
            vertext_end = meshTr.TransformPoint(vertices[i]);

            if (i == 0)
                vertext_start = vertext_end;
            else if (i == 1)
                vertext_middle = vertext_end;
            else
            {
                Vector3 startToMiddle = vertext_middle - vertext_start;
                Vector3 middleToEnd = vertext_end - vertext_middle;

                Vector3 normal = Vector3.Cross(middleToEnd, startToMiddle).normalized;

                vertext_start = vertext_middle;
                vertext_middle = vertext_end;
                normals[i] = normal;
                vertices[i] = vertext_start;
            }
        }
    }

    //void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.yellow;
    //    Vector3 pre = vertices[0];
    //    for (int i = 1; i < vertices.Length; i ++)
    //    {
    //        Gizmos.DrawRay(vertices[i], meshFilter.transform.TransformDirection(normals[i]));
    //        pre = vertices[i];
    //        //Gizmos.DrawRay(vertices[i], normals[i]);
    //    }
    //}
}
