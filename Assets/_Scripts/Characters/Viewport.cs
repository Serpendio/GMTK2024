using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Viewport : MonoBehaviour
{
    public Transform target;
    Camera cam;
    Rigidbody2D rb;
    public float baseSize = 5, trackSpeed = 2;
    public Vector3 offset;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        rb = GetComponent<Rigidbody2D>();
        UpdateScale(1);
    }

    private void FixedUpdate()
    {
        rb.AddForce((target.position - transform.position + offset) * trackSpeed);
    }

    public void UpdateScale(float scale)
    {
        cam.orthographicSize = baseSize + scale - 1;
    }
}
