using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDestroy : MonoBehaviour
{
    private void Awake()
    {
        EnemyHander enemyHander = FindAnyObjectByType<EnemyHander>();
        enemyHander.NubOFEnemys++;
    }

    private void OnDestroy()
    {
        EnemyHander enemyHander = FindAnyObjectByType<EnemyHander>();
        enemyHander.NubOFEnemys--;
    }

}
