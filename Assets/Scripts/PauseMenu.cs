using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Canvas pauseMenu;
    bool pause;
    void Start()
    {
        pauseMenu.gameObject.SetActive(false);
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
        Cursor.lockState = CursorLockMode.None;

        if (pause)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    public void Continue()
    {
        pauseMenu.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
    }
    public void Menu()
    {
        SceneManager.LoadScene(0);
    }
}
