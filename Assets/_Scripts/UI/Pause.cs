using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
                unpause();
            }
            else
            {
                pauseScreenOb.SetActive(true);
                Time.timeScale = 0f;
            }
        }

    }
    public void unpause()
    {
        pauseScreenOb.SetActive(false);
        Time.timeScale = 1.0f;
    }
    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
