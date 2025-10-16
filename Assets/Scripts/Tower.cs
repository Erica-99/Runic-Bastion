#nullable enable
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;


public abstract class Tower : MonoBehaviour
{
    protected abstract float Interval { get; }
    private float timeElapsed = 0;

    [SerializeField]
    [Range(1f, 50f)]
    protected float radius;

    [SerializeField]
    private bool waitForEnemy = true;

    private Material? gizmoMaterial;

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
        if (gizmoMaterial == null)
        {
            string[] ringMatGUIDs = AssetDatabase.FindAssets("RangeRing", new string[] { "Assets/Materials" });
            string path = AssetDatabase.GUIDToAssetPath(ringMatGUIDs[0]);
            Material loadedMaterial = AssetDatabase.LoadAssetAtPath<Material>(path);
            gizmoMaterial = loadedMaterial;
        }

        Matrix4x4 oldMatrix = Gizmos.matrix;
        Matrix4x4 matrix = Matrix4x4.TRS(transform.position + 6f * Vector3.up, Quaternion.identity, Vector3.one);
        Mesh rangeRing = RingMeshGenerator.GenerateMesh(60, radius, 1f, 0.2f);
        gizmoMaterial.SetPass(6);
        Graphics.DrawMeshNow(rangeRing, matrix);
        Gizmos.matrix = oldMatrix;
    }
}
