using System;
using System.Linq;
using UnityEngine;

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
        => UnityEngine.Random.insideUnitCircle.normalized;
    public static Vector2 RandomVector(float maxMagnitude, float minMagnitude = 0)
    {
        var factor = UnityEngine.Random.Range(minMagnitude, maxMagnitude);

        return factor * RandomDirection();
    }
    public static bool IsBlocked(this Vector2 position, Vector2 direction, float offset)
        => Physics2D.RaycastAll(position, direction, offset)
        .FirstOrDefault(c => c.collider != null && c.collider.CompareTag("Ground"));

    /// <summary>
    /// wrapper for <see cref="Physics2D"/> operations
    /// </summary>
    public static PhysicsWrapper Physics(this Vector2 position)
        => new(position);
    public struct PhysicsWrapper
    {
        readonly Vector2 position;
        public PhysicsWrapper(Vector2 position)
        {
            this.position = position;
        }
        public Collider2D[] OverlapCircleAll(float radius)
            => Physics2D.OverlapCircleAll(position, radius) ?? Array.Empty<Collider2D>();
        public RaycastHit2D Raycast(Vector2 direction, float distance)
            => Physics2D.Raycast(position, direction, distance);
    }

}
