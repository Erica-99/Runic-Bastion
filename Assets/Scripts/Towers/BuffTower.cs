using UnityEngine;

namespace RunicBastion.Towers
{
    public class BuffTower : Tower
    {
        public GameObject projectile;

        protected override float Interval { get; } = 10f;

        protected override void Behaviour(GameObject target)
        {
            GameObject ring = Instantiate(projectile, transform, false);
            ring.GetComponent<BuffSpell>().Release(radius);
        }
    }
}