#nullable enable
using System.Collections.Generic;
using UnityEngine;


public abstract class Tower : MonoBehaviour
{
    protected abstract float Interval { get; }
    private float timeElapsed = 0;

    [SerializeField]
    [Range(1f, 50f)]
    protected float radius;

    [SerializeField]
    private bool waitForEnemy = true;

    private Color gizmoColour = new Color(1f, 0f, 0f, 0.5f);

    void FixedUpdate()
    {
        DoTimer();
    }

    private void DoTimer()
    {
        if (timeElapsed > Interval)
        {
            if (waitForEnemy)
            {
                GameObject? target = GetPriorityEnemy();
                if (target != null)
                {
                    timeElapsed = 0;
                    Behaviour(target);
                }
            }
        }
        else
        {
            timeElapsed += Time.deltaTime;
        }
    }

    protected abstract void Behaviour(GameObject target);

    private GameObject? GetPriorityEnemy()
    {
        GameObject? returnEnemy = null;

        Collider[] hitColliders = Physics.OverlapSphere(transform.position + new Vector3(0f, 6f, 0f), radius);

        GameObject? bestFoundObject = null;
        float bestDistance = 1000;
        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.CompareTag("Enemy"))
            {
                EnemyMovement? enemyScript = hitCollider.gameObject.GetComponent<EnemyMovement>();
                if (enemyScript != null && enemyScript.DistToCrystal < bestDistance)
                {
                    bestDistance = enemyScript.DistToCrystal;
                    bestFoundObject = hitCollider.gameObject;
                }
            }
        }
        returnEnemy = bestFoundObject;

        return returnEnemy;

    }

    private void OnDrawGizmosSelected()
    {
        Matrix4x4 oldMatrix = Gizmos.matrix;
        Gizmos.color = gizmoColour;
        Gizmos.matrix = Matrix4x4.TRS(transform.position + new Vector3(0f, 6f, 0f), transform.rotation, new Vector3(1, 0.02f, 1));
        Gizmos.DrawSphere(Vector3.zero, radius);
        Gizmos.matrix = oldMatrix;
    }
}
