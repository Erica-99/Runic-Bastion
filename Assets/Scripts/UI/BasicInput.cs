using UnityEngine;

public class BasicInput : MonoBehaviour
{
    pauseMenu Pause;

    void Start()
    {
        Pause = GameObject.FindGameObjectWithTag("Pause").GetComponent<pauseMenu>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu BasicInput = Pause;
        }
    }
}
