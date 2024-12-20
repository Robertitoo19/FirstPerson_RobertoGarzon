using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManage : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject audioMenu;
    [SerializeField] private GameObject controlsMenu;
    public void Play()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1.0f;
    }
    public void Options()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
        audioMenu.SetActive(false);
        controlsMenu.SetActive(false);
    }
    public void Menu()
    {
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
        audioMenu.SetActive(false);
        controlsMenu.SetActive(false);
    }
    public void Audio()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
        audioMenu.SetActive(true);
        controlsMenu.SetActive(false);
    }
    public void Controls()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
        audioMenu.SetActive(false);
        controlsMenu.SetActive(true);
    }
}
