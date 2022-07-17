using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraManager : MonoBehaviour
{
    public Vector2 angles;
    public Vector3 oriPosition;
    public Quaternion oriQuaternion;
    public Transform player;
    public Transform oriTr;
    
    void Start()
    {
        StartCoroutine(MoveOnStart());
    }

    public void MoveCam()
    {
        StartCoroutine(MoveOnEnd());
        //oriPosition = transform.localPosition;
        //oriQuaternion = transform.localRotation;
        //transform.parent = null;

        //transform.position = oriTr.position;
        //transform.rotation = oriTr.rotation;
    }
    IEnumerator MoveOnStart()
    {
        oriPosition = transform.localPosition;
        oriQuaternion = transform.localRotation;
        transform.parent = null;

        transform.position = oriTr.position;
        transform.rotation = oriTr.rotation;

        while (!GameManager.instance.gameStarted)
            yield return null;
        transform.SetParent(player);
        while (Vector3.Distance(transform.localPosition, oriPosition) >= 0.01f)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, oriPosition, 0.025f);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, oriQuaternion, 0.05f);
            yield return null;
        }

        angles.y = transform.localEulerAngles.x;
        angles.x = transform.localEulerAngles.y;
    }
    IEnumerator MoveOnEnd()
    {
        transform.parent = null;

        transform.position = oriTr.position;
        transform.rotation = oriTr.rotation;

        while (!GameManager.instance.gameStarted)
            yield return null;
        transform.SetParent(player);
        while (Vector3.Distance(transform.localPosition, oriPosition) >= 0.01f)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, oriPosition, 0.025f);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, oriQuaternion, 0.05f);
            yield return null;
        }

        angles.y = transform.localEulerAngles.x;
        angles.x = transform.localEulerAngles.y;
    }
    // Update is called once per frame
    void Update()
    {
        if(GameManager.instance.gameStarted)
        {
            float rh = Input.GetAxis("Mouse X");
            float rv = Input.GetAxis("Mouse Y");

            angles.x += rh * 2;
            angles.y -= rv * 2;
            transform.localRotation = Quaternion.Euler(angles.y, angles.x, 0);
        }
        else
        {

        }
        //transform.Rotate(transform.parent.forward,-rh);
    }
}
