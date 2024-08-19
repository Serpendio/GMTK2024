using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDestroy : MonoBehaviour
{
    private void Awake()
    {
        EnemyHander enemyHander = FindAnyObjectByType<EnemyHander>();
        if (enemyHander != null)
        {
            enemyHander.NubOFEnemys++;
        }
    }

    private void OnDestroy()
    {
        EnemyHander enemyHander = FindAnyObjectByType<EnemyHander>();
        if (enemyHander != null)
        {
            enemyHander.NubOFEnemys--;
        }
    }

}
