using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBounds : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.parent.TryGetComponent(out IOnDeath onDeath))
        {
            onDeath.Die(collision.rigidbody);
        };
    }
}
