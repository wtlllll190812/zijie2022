using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Curve : MonoBehaviour
{
    public MeshFilter meshFilter;
    public Transform pos;
    public float speed;
    public bool canMove = true;


    private Vector3[] normals;
    private Vector3[] vertices;
    private Vector3 targetPos;

    void Start()
    {
        Calculate();
        targetPos = transform.position;
        StartCoroutine(Move());
    }
    
    void FixedUpdate()
    {
        transform.position=Vector3.Lerp(transform.position, targetPos, speed);
    }
    private IEnumerator Move()
    {
        int count = vertices.Length - 1;
        Quaternion q=Quaternion.identity;
        while (canMove)
        {
            if (count <= 0)
                count = vertices.Length - 1;
            targetPos = vertices[count];
            Debug.Log(transform.forward);
            q.SetLookRotation(normals[count], transform.up);
            transform.rotation = q;
            //transform.LookAt(transform.position + Vector3.Cross(normals[count], transform.right), normals[count]);
            count -= 2;
            yield return new WaitForSeconds(0.1f);
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
            {
                vertext_start = vertext_end;
                continue;
            }
            else if (i == 1)
            {
                vertext_middle = vertext_end;
                continue;
            }
            Vector3 startToMiddle = vertext_middle - vertext_start;
            Vector3 middleToEnd = vertext_end - vertext_middle;

            Vector3 normal = Vector3.Cross(middleToEnd, startToMiddle).normalized;

            vertext_start = vertext_middle;
            vertext_middle = vertext_end;
            normals[i] = normal;
            vertices[i] = vertext_start;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        for (int i = 0; i < vertices.Length; i += 2)
        {
            Gizmos.DrawRay(vertices[i], normals[i]);
        }
    }
}
