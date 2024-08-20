using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.UI.Image;

public static class Vector2Ext
{
    public static Vector2 VectorTo(this Vector2 from, Vector2 to)
        => to - from;
    public static Vector2 DirectionTo(this Vector2 from, Vector2 to)
        => from.VectorTo(to).normalized;
    public static Vector2 With(this Vector2 original, float? x = null, float? y = null)
        => new Vector2(
            (float)(x == null ? original.x : x),
            (float)(y == null ? original.y : y)
            );
    public static float Distance(this Vector2 from, Vector2 to)
        => Vector2.Distance(from, to);
    public static Vector3 To3D(this Vector2 vector, float? z = null)
        => new Vector3(vector.x, vector.y, (float)(z == null ? 0 : z));
    public static Vector2 RandomDirection()
        => Random.insideUnitCircle.normalized;
    public static Vector2 RandomVector(float maxMagnitude, float minMagnitude = 0)
    {
        var factor = Random.Range(minMagnitude, maxMagnitude);

        return factor * RandomDirection();
    }
    public static bool IsBlocked(this Vector2 position, Vector2 direction, float offset)
        => Physics2D.RaycastAll(position, direction, offset)
        .FirstOrDefault(c => c.collider != null && c.collider.CompareTag("Ground"));
}
