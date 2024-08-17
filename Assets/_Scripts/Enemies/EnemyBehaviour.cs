using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyBehaviour : MonoBehaviour
{
    Enemy enemy;
    private void Awake()
    {
        enemy = GetComponent<Enemy>();
    }
}
