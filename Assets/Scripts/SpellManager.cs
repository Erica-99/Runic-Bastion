using System.Collections.Generic;
using UnityEngine;

public class SpellManager : MonoBehaviour
{
    private List<GameObject> spells;

    private Manager gameManager;

    public GameObject paper;
    public GameObject drawingSphere;

    public bool showDebug;

    void Awake()
    {
        GameObject manager = GameObject.FindGameObjectWithTag("GameController");
        gameManager = manager.GetComponent<Manager>();

        spells = new List<GameObject>();
    }

    public void createSpells()
    {
        foreach (GameObject spellPrefab in gameManager.spellPrefabs)
        {
            GameObject newSpell = Instantiate(spellPrefab, paper.transform);
            newSpell.GetComponent<Rune>().Initialize(drawingSphere, showDebug);
            spells.Add(newSpell);
        }
    }

    void CheckSpellCompletion()
    {
        List<GameObject> completedSpells = new List<GameObject>();

        foreach (GameObject spell in spells)
        {
            Rune spellclass = spell.GetComponent<Rune>();
            if (spellclass.completed)
            {
                completedSpells.Add(spell);
            }
        }

        Rune selectedSpell = null;
        int currentPriority = -1000;

        if (completedSpells.Count > 0)
        {
            foreach (GameObject spell in completedSpells)
            {
                if (spell.GetComponent<Rune>().Priority > currentPriority)
                {
                    selectedSpell = spell.GetComponent<Rune>();
                }
            }
        }

        if (selectedSpell != null)
        {
            selectedSpell.DoSpell();
        }
        else
        {
            //IDK burn the paper or something. Sort this out later.
        }
        
        ClearAllSpells();
    }

    private void ClearAllSpells()
    {
        foreach (GameObject spell in spells)
        {
            spell.GetComponent<Rune>().ClearSpheres();
            Destroy(spell);
        }

        spells.Clear();
    }

    private void OnEnable()
    {
        gameManager.CheckSpells += CheckSpellCompletion;
    }

    private void OnDisable()
    {
        gameManager.CheckSpells -= CheckSpellCompletion;
    }

}
