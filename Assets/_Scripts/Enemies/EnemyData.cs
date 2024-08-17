
using UnityEngine;

[CreateAssetMenu(menuName ="ScriptableObject/EnemyData")]
public class EnemyData : ScriptableObject
{
    [SerializeField] private float size;
    [SerializeField] private float speed;
    [SerializeField] private Color color;
}
