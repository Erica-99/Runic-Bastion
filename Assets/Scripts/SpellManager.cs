using System.Collections.Generic;
using UnityEngine;

public class SpellManager : MonoBehaviour
{
    private List<GameObject> spells;

    private Manager gameManager;

    public GameObject paper;
    public GameObject drawingSphere;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
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
            newSpell.GetComponent<Rune>().Initialize(drawingSphere, debug:true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
