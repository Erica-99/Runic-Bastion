using UnityEngine;
using UnityEngine.InputSystem;

public class Manager : MonoBehaviour
{
    public bool castMode = false;

    private InputAction enableCastModeAction;

    private void Awake()
    {
        enableCastModeAction = InputSystem.actions.FindAction("Cast");
    }

    // Update is called once per frame
    void Update()
    {
        if (enableCastModeAction.WasPressedThisFrame())
        {
            castMode = true;
            Cursor.lockState = CursorLockMode.Confined;

        } else if (enableCastModeAction.WasReleasedThisFrame())
        {
            castMode = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
