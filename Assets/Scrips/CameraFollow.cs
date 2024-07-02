using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    Transform target;
    Vector3 offset;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        offset = transform.position - target.position;
    }

    void LateUpdate()
    {
        Vector3 targetPos = target.position + offset;
        transform.position=Vector3.Lerp(transform.position, targetPos, Time.deltaTime);
    }
}
