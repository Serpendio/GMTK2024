using UnityEngine;

public static class TransformExt
{
    public static bool IsGrounded(this Transform transform, Collider2D collider, float offset)
        => transform.position.IsGrounded(collider, offset);
}
