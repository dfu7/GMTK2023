using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;
    public GameObject HowTo;

    private void Update()
    {
        if (GameIsPaused)
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                LoadMenu();
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                DisplayControls();
            }
            if (Input.GetKeyDown(KeyCode.X))
            {
                CloseControls();
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        CloseControls();
        Debug.Log("pause clicked");
        Cursor.lockState = CursorLockMode.Confined;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        Cursor.lockState = CursorLockMode.None;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void DisplayControls()
    {
        Debug.Log("display ctrls clicked");
        HowTo.SetActive(true);
    }

    public void CloseControls()
    {
        Debug.Log("close ctrls clicked");
        HowTo.SetActive(false);
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Start");
    }
}
