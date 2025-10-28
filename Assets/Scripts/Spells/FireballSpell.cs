using UnityEngine;
using System.Collections.Generic;

public class FireballSpell : Rune
{
    public override int Priority { get; set; } = 0;

    public GameObject fireballProjectile;

    private void Awake()
    {
        // Scaled to [0,1] x and y on the drawable section of the paper. This will be resized automatically to the paper.
        // (0,0) is bottom right and (1,1) is top left.
        customRune = new List<Vector2>()
        {
            new Vector2(0.8f, 0.5f),
            new Vector2(0.5f, 0.1f),
            new Vector2(0.2f, 0.5f),
            new Vector2(0.5f, 0.9f),
            new Vector2(0.8f, 0.5f)
        };
    }

    public override void DoSpell()
    {
        Debug.Log("Casting fireball.");

        // Load the fireball projectile object/script into the attack manager
        Manager manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<Manager>();

        AttackManager atkman = manager.WizardController.GetComponent<AttackManager>();

        GameObject instance = Instantiate(fireballProjectile);

        atkman.LoadAttack(instance);

        atkman.ReadyAttack();
    }
}
