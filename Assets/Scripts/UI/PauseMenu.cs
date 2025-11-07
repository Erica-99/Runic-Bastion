using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class pauseMenu : MonoBehaviour
{

    [SerializeField] GameObject pauseMenuObject;
public void Pause()
{
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
}

public void Home()
{
        SceneManager.LoadScene("Main Menu");
}

public void Resume()
{
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
}

    internal static void SetActive(bool v)
    {
        throw new NotImplementedException();
    }
}
