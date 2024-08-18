using System.Collections;
using System.Collections.Generic;
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
    public static double Distance(this Vector2 from, Vector2 to)
        => Vector2.Distance(from, to);
    public static Vector3 To3D(this Vector2 vector, float? z = null)
        => new Vector3(vector.x, vector.y, (float)(z == null ? 0 : z));
}
