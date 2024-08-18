using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public static Vector2 To2D(this Vector3 vector)
        => (Vector2)vector;


    public static bool IsGrounded(this Vector3 position, float offset)
        => Physics2D.RaycastAll(position, Vector3.down, offset)
        .FirstOrDefault(c => c.collider != null && c.collider.CompareTag("Ground"));
    public static bool IsBlocked(this Vector3 position, Vector3 direction,float offset)
        => Physics2D.RaycastAll(position, direction, offset)
        .FirstOrDefault(c => c.collider != null && c.collider.CompareTag("Ground"));
    public static bool IsGrounded(this Vector3 position, Collider2D collider, float offset)
        => position.IsGrounded(collider.bounds.extents.y + offset);
    public static bool IsBlocked(this Vector3 position, Collider2D collider, float offset)
        => position.IsBlocked(position.DirectionTo(collider.transform.position),collider.bounds.extents.x + offset);

}
