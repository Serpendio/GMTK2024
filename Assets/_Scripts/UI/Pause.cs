using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    [SerializeField] GameObject pauseScreenOb;


    private void Start()
    {
        pauseScreenOb.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseScreenOb.activeInHierarchy == true)
            {
                pauseScreenOb.SetActive(false);
                Time.timeScale = 1.0f;
            }
            else
            {
                pauseScreenOb.SetActive(true);
                Time.timeScale = 0f;
            }
        }

    }
}
