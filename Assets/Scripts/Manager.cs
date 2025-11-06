using System.Collections.Generic;
using TMPro;
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

    private int pages = 3;
    public float timeBetweenPages = 7f;
    private float currentPageTimeElapsed = 0f;

    private WaveSpawner waveSpawner;
    public TextMeshProUGUI pagesTextbox;

    private InputAction freePagesButton;

    public void UsePage()
    {
        pages--;
        if (pages <= 0)
        {
            pages = 0;
        } 
    }

    private void AddPage()
    {
        pages++;
    }

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
        waveSpawner = GetComponent<WaveSpawner>();
        freePagesButton = InputSystem.actions.FindAction("PagesCheat");
    }

    // Update is called once per frame
    void Update()
    {
        if (waveSpawner.gameEnded)
        {
            castMode = false;
            Cursor.lockState = CursorLockMode.Confined;

        } else if (enableCastModeAction.WasPressedThisFrame() && pages > 0)
        {
            castMode = true;
            Cursor.lockState = CursorLockMode.Confined;

        } else if (enableCastModeAction.WasReleasedThisFrame())
        {
            castMode = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        if (!waveSpawner.gameEnded)
        {
            if (currentPageTimeElapsed > timeBetweenPages)
            {
                AddPage();
                currentPageTimeElapsed = 0f;
            } else
            {
                currentPageTimeElapsed += Time.deltaTime;
            }
        }

        pagesTextbox.text = pages.ToString();
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

    private void GiveFreePages(InputAction.CallbackContext context)
    {
        pages += 10;
    }

    private void OnEnable()
    {
        freePagesButton.performed += context => GiveFreePages(context);
    }

    private void OnDisable()
    {
        freePagesButton.performed -= context => GiveFreePages(context);
    }
}
