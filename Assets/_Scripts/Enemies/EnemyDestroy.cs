using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDestroy : MonoBehaviour
{
    private void OnDestroy()
    {
        EnemyHander enemyHander = FindAnyObjectByType<EnemyHander>();
        enemyHander.NubOFEnemys--;
    }

}
