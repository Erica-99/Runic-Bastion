using System.Collections.Generic;
using UnityEngine;

public class BuffTowerSpell : Rune
{
    public override int Priority { get; set; } = 0;

    public GameObject BuffTower;

    private void Awake()
    {
        // Scaled to [0,1] x and y on the drawable section of the paper. This will be resized automatically to the paper.
        // (0,0) is bottom right and (1,1) is top left.
        customRune = new List<Vector2>() {
        new Vector2(0.2f, 0.1f),
        new Vector2(0.8f, 0.1f),
        new Vector2(0.2f, 0.5f),
        new Vector2(0.8f, 0.9f)
        };
    }

    public override void DoSpell()
    {
        Debug.Log("BUFF TOWER :)");

        Manager manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<Manager>();

        AttackManager atkman = manager.WizardController.GetComponent<AttackManager>();

        GameObject instance = Instantiate(BuffTower);

        atkman.LoadAttack(instance);

        atkman.ReadyAttack();
    }
}