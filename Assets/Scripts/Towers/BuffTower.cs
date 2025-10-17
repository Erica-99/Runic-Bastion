using UnityEngine;

public class BuffTower : Tower
{
    public GameObject projectile;

    protected override float Interval { get; } = 5f;

    protected override void Behaviour(GameObject target)
    {
        GameObject ring = Instantiate(projectile, transform, false);
        ring.GetComponent<BuffSpell>().Release(radius);
    }
}
