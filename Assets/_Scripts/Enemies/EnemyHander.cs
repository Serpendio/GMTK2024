using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class EnemyHander : MonoBehaviour
{
    public int NubOFEnemys;
    private void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
       if (NubOFEnemys == 0)
        {
            SceneManager.LoadScene(0);
        }
    }
}
