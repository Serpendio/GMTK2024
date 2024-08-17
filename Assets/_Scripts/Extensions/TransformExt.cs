using UnityEngine;

public static class TransformExt
{
    public static float Distance(this Transform from, Transform to)
        => from.position.Distance(to.position);
    public static bool IsGrounded(this Transform transform, Collider2D collider, float offset)
        => transform.position.IsGrounded(collider, offset);
}
