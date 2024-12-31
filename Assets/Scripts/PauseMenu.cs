using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Canvas pauseMenu;
    [SerializeField] private Canvas mainCanvas;
    bool pause;
    void Start()
    {
        pauseMenu.gameObject.SetActive(false);
        mainCanvas.gameObject.SetActive(true);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            Pausar();
        }
    }
    private void Pausar()
    {
        pause = !pause;
        pauseMenu.gameObject.SetActive(pause);
        mainCanvas.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.None;

        if (pause)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            mainCanvas.gameObject.SetActive(true);
        }
    }
    public void Continue()
    {
        pauseMenu.gameObject.SetActive(false);
        mainCanvas.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
    }
    public void Menu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
    }
}
