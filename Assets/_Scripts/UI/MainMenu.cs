using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject settings;
    [SerializeField] GameObject credis;
    [SerializeField] GameObject LoadLevel;
    private void Start()
    {
        mainMenu.SetActive(true);
        settings.SetActive(false);
        credis.SetActive(false);
        LoadLevel.SetActive(false);
    }
    public void StartGame()
    {
        mainMenu.SetActive(false);
        LoadLevel.SetActive(true);
    }
    public void LoadLever(int nub)
    {
        SceneManager.LoadScene(nub);
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
        LoadLevel.SetActive(false);
        settings.SetActive(false);
        credis.SetActive(false);
        mainMenu.SetActive(true);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
