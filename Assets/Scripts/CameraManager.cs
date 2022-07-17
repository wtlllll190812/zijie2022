using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float rh = Input.GetAxis("Mouse X");
        float rv = Input.GetAxis("Mouse Y");

        transform.localRotation *= Quaternion.AngleAxis(rh,Vector3.up);
        transform.localRotation *= Quaternion.AngleAxis(-rv,Vector3.right);
        //transform.Rotate(transform.parent.forward,-rh);
    }
}
