using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eyes : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float rotationSpeed = 1f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //rb.AddTorque((transform.parent.rotation.z + transform.rotation.z + 90) * rotationSpeed * Time.deltaTime);
        transform.localPosition = Vector3.zero;
        //transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
