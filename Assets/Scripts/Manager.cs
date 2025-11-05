using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Manager : MonoBehaviour
{
    public bool castMode = false;

    private InputAction enableCastModeAction;

    public List<GameObject> spellPrefabs;

    public GameObject WizardController;

    public event System.Action CheckSpells;
    private bool spellInvokeQueued = false;

    public GameObject crosshair;

    public void SetCrosshairState(bool state)
    {
        crosshair.SetActive(state);
    }

    public void triggerSpellCheck()
    {
        spellInvokeQueued = true;
    }

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

    // Forced to activate at the end of the frame so that it only fires once even if multiple spells are fulfilled somehow.
    private void LateUpdate()
    {
        if (spellInvokeQueued)
        {
            spellInvokeQueued = false;
            CheckSpells?.Invoke();
        }
    }
}
