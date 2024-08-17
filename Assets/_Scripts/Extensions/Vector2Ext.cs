using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector2Ext
{
    public static Vector2 VectorTo(this Vector2 from, Vector2 to)
        => to - from;
    public static Vector2 DirectionTo(this Vector2 from, Vector2 to)
        => from.VectorTo(to).normalized;
}
