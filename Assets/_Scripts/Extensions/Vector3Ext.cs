using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class Vector3Ext
{
    public static Vector3 VectorTo(this Vector3 from, Vector3 to)
        => to - from;
    public static Vector3 DirectionTo(this Vector3 from, Vector3 to)
        => from.VectorTo(to).normalized;
    public static Vector3 With(this Vector3 original, float? x = null, float? y = null, float? z = null)
        => new Vector3(
            (float)(x == null ? original.x : x),
            (float)(y == null ? original.y : y),
            (float)(z == null ? original.z : z)
            );

    public static float Distance(this Vector3 from, Vector3 to)
        => Vector3.Distance(from, to);

    public static bool IsGrounded(this Vector3 position, float offset)
        => Physics2D.Raycast(position, Vector3.down, offset);
    public static bool IsGrounded(this Vector3 position, Collider2D collider, float offset)
        => position.IsGrounded(collider.bounds.extents.y + offset);
}
