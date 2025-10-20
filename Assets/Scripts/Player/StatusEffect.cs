using System.Collections.Generic;
using UnityEngine;

public enum Stat
{
    Speed,
    Jump,
    Attack
}

public class StatusEffect
{
    private Dictionary<Stat, float> StatChanges { get; }

    private float totalDuration;
    public float timeRemaining;

    public StatusEffect(Dictionary<Stat, float> statChanges, float totalDuration)
    {
        StatChanges = statChanges;
        this.totalDuration = totalDuration;
    }

    public void StartEffect(BuffManager buffManager)
    {
        foreach (Stat key in StatChanges.Keys)
        {
            buffManager.Stats[(int)key] += StatChanges[key];
        }

        timeRemaining = totalDuration;
    }

    public void EndEffect(BuffManager buffManager)
    {
        foreach(Stat key in StatChanges.Keys)
        {
            buffManager.Stats[(int)key] -= StatChanges[key];
        }
    }
}
