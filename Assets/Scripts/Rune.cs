using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

abstract public class Rune : MonoBehaviour
{

    private List<GameObject> drawPoints;
    private int nextPoint;

    public bool completed;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        drawPoints = new List<GameObject>();
    }

    public void Initialize(List<Vector3> drawPointPositions, GameObject pointPrefab, bool debug=false)
    {
        foreach (Vector3 point in drawPointPositions)
        {
            GameObject newSphere = Instantiate(pointPrefab, point, Quaternion.identity, transform);
            if (debug)
            {
                newSphere.GetComponent<MeshRenderer>().enabled = true;
            }

            drawPoints.Add(newSphere);
        }

        nextPoint = 0;
    }

    abstract public void DoSpell();

    void ClearSpheres()
    {
        foreach (GameObject point in drawPoints)
        {
            Destroy(point);
        }
        drawPoints.Clear();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Collider sphereCollider = collision.GetContact(0).thisCollider;

        if ( sphereCollider == drawPoints[nextPoint].GetComponent<Collider>())
        {
            drawPoints[nextPoint].GetComponent<Renderer>().material.color = new Color(0f, 1f, 0f, 0.75f);

            nextPoint += 1;
            if (nextPoint >= drawPoints.Count)
            {
                completed = true;
            }
        }
    }
}
