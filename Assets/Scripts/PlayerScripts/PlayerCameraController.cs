using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{

    Camera _camera;
    [Range(min: 0.01f, max: 1)]
    public float RangeChangeSpeed = 0.2f;
    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }
    void Update()
    {
    }

    public void MoveForward()
    {
        Vector3 lastpos = transform.localPosition;
        lastpos.z += 0.2f;
        transform.localPosition = lastpos;
    }
    public void MoveBackward()
    {
        Vector3 lastpos = transform.localPosition;
        lastpos.z -= 0.2f;
        transform.localPosition = lastpos;
    }

}
