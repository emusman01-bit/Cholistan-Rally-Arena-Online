using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lerper : MonoBehaviour
{
    public Transform target;
    public float lerpValuePos=0.5f;
    public float lerpValueRot=0.5f;

    void FixedUpdate()
    {
        if(!target) return;
        transform.position = Vector3.Lerp(transform.position, target.position, lerpValuePos);
        var rot = transform.rotation;
        rot.x = Mathf.Lerp(rot.x, target.rotation.x, lerpValueRot);
        rot.y = Mathf.Lerp(rot.y, target.rotation.y, lerpValueRot);
        rot.z = Mathf.Lerp(rot.z, target.rotation.z, lerpValueRot);
        rot.w = Mathf.Lerp(rot.w, target.rotation.w, lerpValueRot);
        transform.rotation = rot;
        // transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, target.eulerAngles, lerpValuePos);      
    }
}
