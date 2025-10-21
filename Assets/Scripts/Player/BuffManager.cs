using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    private float[] _stats = new float[3];

    public float[] Stats
    {
        get { return _stats; }
        set { _stats = value; }
    }

    private void Awake()
    {
        Stats[(int)Stat.Speed] = 1f;
        Stats[(int)Stat.Jump] = 1f;
        Stats[(int)Stat.Attack] = 1f;
    }

    private List<StatusEffect> currentEffects = new();

    public CharacterMovement movementScript;
    public AttackManager attackScript;

    public void ApplyStatusEffect(StatusEffect statusEffect)
    {
        if (statusEffect == null) return;

        statusEffect.StartEffect(this);
        currentEffects.Add(statusEffect);
    }

    public void RemoveStatusEffect(StatusEffect statusEffect)
    {
        if (statusEffect == null || !currentEffects.Contains(statusEffect)) return;

        statusEffect.EndEffect(this);
        currentEffects.Remove(statusEffect);
    }

    public void UpdateEffectTimers()
    {
        foreach (StatusEffect statusEffect in currentEffects)
        {
            if (statusEffect != null)
            {
                statusEffect.timeRemaining -= Time.deltaTime;
            }
        }
    }

    private List<StatusEffect> GetRemovableEffects()
    {
        List<StatusEffect > effects = new List<StatusEffect>();

        foreach (StatusEffect statusEffect in currentEffects)
        {
            if (statusEffect.timeRemaining < 0f)
            {
                effects.Add(statusEffect);
            }
        }

        return effects;
    }

    private void LateUpdate()
    {
        UpdateEffectTimers();

        foreach (StatusEffect statusEffect in GetRemovableEffects())
        {
            RemoveStatusEffect(statusEffect);
            currentEffects.Remove(statusEffect);
        }

        movementScript.speedBuff = Stats[(int)Stat.Speed];
        movementScript.jumpBuff = Stats[(int)Stat.Jump];
        attackScript.attackBuff = Stats[(int)Stat.Attack];
    }
}
