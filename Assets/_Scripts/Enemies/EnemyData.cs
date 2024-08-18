
using UnityEngine;

[CreateAssetMenu(menuName ="ScriptableObject/EnemyData")]
public class EnemyData : ScriptableObject
{
    public float Size;
    public float Speed;
    public float JumpForce;
    public float AwarenessRange;
    public float IdleRange;
}
