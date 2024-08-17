using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Viewport : MonoBehaviour
{
    public Transform target;
    Camera cam;
    public float baseSize = 5;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        UpdateScale(1);
    }

    private void Update()
    {
        transform.position = target.position + Vector3.back * 10;
    }

    public void UpdateScale(float scale)
    {
        cam.orthographicSize = baseSize + scale - 1;
    }
}
