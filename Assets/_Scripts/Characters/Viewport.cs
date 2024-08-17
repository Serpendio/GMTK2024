using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Viewport : MonoBehaviour
{
    public Transform target;
    Camera cam;
    Rigidbody2D rb;
    public float baseSize = 5, trackSpeed = 2;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        rb = GetComponent<Rigidbody2D>();
        UpdateScale(1);
    }

    private void Update()
    {
        rb.AddForce((target.position + Vector3.back * 10 - transform.position) * trackSpeed);
    }

    public void UpdateScale(float scale)
    {
        cam.orthographicSize = baseSize + scale - 1;
    }
}
