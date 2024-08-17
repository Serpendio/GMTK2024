using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector3Ext
{
    public static bool IsGrounded(this Vector3 position, float offset)
        => Physics2D.Raycast(position, -Vector3.up, offset);
    public static bool IsGrounded(this Vector3 position, Collider2D collider, float offset)
        => position.IsGrounded(collider.bounds.extents.y + offset);
}
