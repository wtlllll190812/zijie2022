using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraManager : MonoBehaviour
{
    public Vector2 angles;

    void Start()
    {
        angles.y = transform.localEulerAngles.x;
        angles.x = transform.localEulerAngles.y;
    }

    // Update is called once per frame
    void Update()
    {
        float rh = Input.GetAxis("Mouse X");
        float rv = Input.GetAxis("Mouse Y");

        angles.x += rh;
        angles.y -= rv;
        transform.localRotation = Quaternion.Euler(angles.y, angles.x, 0);
        //transform.Rotate(transform.parent.forward,-rh);
    }
}
