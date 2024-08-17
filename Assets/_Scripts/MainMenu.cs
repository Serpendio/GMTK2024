using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject settings;
    [SerializeField] GameObject credis;
    private void Start()
    {
        mainMenu.SetActive(true);
        settings.SetActive(false);
        credis.SetActive(false);
    }
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
    public void Settings()
    {

    }
    public void Credis()
    {
        mainMenu.SetActive(false);
        credis.SetActive(true);
    }
    public void backToMenu()
    {
        settings.SetActive(false);
        credis.SetActive(false);
        mainMenu.SetActive(true);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
