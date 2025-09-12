using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class Manager : MonoBehaviour
{
    public bool castMode = false;

    private InputAction enableCastModeAction;

    public List<GameObject> spellPrefabs;

    private void Awake()
    {
        enableCastModeAction = InputSystem.actions.FindAction("Cast");

        // Load in spells from the folder.
        string[] prefabGUIDs = AssetDatabase.FindAssets("t:prefab", new string[] { "Assets/Prefabs/Spells" });
        foreach (string guid in prefabGUIDs)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject loadedAsset = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            spellPrefabs.Add(loadedAsset);
        }
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
