using UnityEngine;

namespace RunicBastion.Towers
{
    public class PlantTower : Tower
    {
        public GameObject projectile;

        protected override float Interval { get; } = 7f;

        protected override void Behaviour(GameObject target)
        {
            GameObject vine = Instantiate(projectile, transform, false);
            vine.GetComponent<VineBehaviour>().Release(target);
        }
    }
}