using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class pauseMenu : MonoBehaviour
{

    [SerializeField] GameObject pauseMenuObject;

    private InputAction pauseAction;
    private bool isPaused = false;
    private bool queueLock = false;

    private void Awake()
    {
        pauseAction = InputSystem.actions.FindAction("Pause");
    }

    public void Pause()
    {
        pauseMenuObject.SetActive(true);
        Time.timeScale = 0;
        isPaused = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void Home()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Confined;
        SceneManager.LoadScene("main menu");
    }

    public void Resume()
    {
        pauseMenuObject.SetActive(false);
        Time.timeScale = 1;
        isPaused = false;
        queueLock = true;
    }

    private void TogglePause()
    {
        Debug.Log("Pause");
        if (isPaused)
        {
            Resume();
        } else
        {
            Pause();
        }
    }

    private void OnEnable()
    {
        pauseAction.performed += context => TogglePause();
    }

    private void OnDisable()
    {
        pauseAction.performed += context => TogglePause();
    }

    private void LateUpdate()
    {
        if (queueLock)
        {
            Cursor.lockState = CursorLockMode.Locked;
            queueLock = false;
        }
    }
}
