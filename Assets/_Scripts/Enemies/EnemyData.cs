
using UnityEngine;

[CreateAssetMenu(menuName ="ScriptableObject/EnemyData")]
public class EnemyData : ScriptableObject
{
    public float Size = 1;
    public float Speed;
    public float JumpForce;
    public float AwarenessRange;
    public float IdleRange;
    public float IdleSeconds = 1;
    public float IdleSpeedFactor = 1;
}
