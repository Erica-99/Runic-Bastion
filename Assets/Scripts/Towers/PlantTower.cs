using UnityEngine;

public class PlantTower : Tower
{
    public GameObject projectile;

    protected override float Interval { get; } = 7f;


    protected override void Behaviour(GameObject target)
    {
        GameObject vine = Instantiate(projectile, transform, false);
        vine.transform.rotation = Quaternion.LookRotation(target.transform.position);
    }
}
