using System;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class MovementSpell : Rune
{
    public override int Priority { get; set; } = 0;

    [Serializable]
    public struct buffDefinition
    {
        public Stat stat;
        public float amount;
    }

    public float buffDuration;

    public buffDefinition[] buffToApply;

    public Dictionary<Stat, float> buff = new Dictionary<Stat, float>();



    private void Awake()
    {
        // Scaled to [0,1] x and y on the drawable section of the paper. This will be resized automatically to the paper.
        // (0,0) is bottom right and (1,1) is top left.
        customRune = new List<Vector2>() {
        new Vector2(0.5f, 0.1f),
        new Vector2(0.5f, 0.9f),
        new Vector2(0.8f, 0.6f),
        new Vector2(0.2f, 0.6f),
        new Vector2(0.5f, 0.9f)
        };
    }

    public override void DoSpell()
    {
        Debug.Log("Movement speed buffed");

        foreach (buffDefinition definedBuff in buffToApply)
        {
            buff[definedBuff.stat] = definedBuff.amount;
        }

        StatusEffect statusEffect = new StatusEffect(buff, buffDuration);

        Manager manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<Manager>();

        BuffManager buffManager = manager.WizardController.GetComponent<BuffManager>();

        buffManager.ApplyStatusEffect(statusEffect);
    }
}
