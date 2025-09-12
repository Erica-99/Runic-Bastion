using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

abstract public class Rune : MonoBehaviour
{

    private List<GameObject> drawPoints;
    private int nextPoint;

    private GameObject manager;
    protected Manager managerScript;

    public bool completed;

    List<Vector2> customRune;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        drawPoints = new List<GameObject>();
        manager = GameObject.FindGameObjectWithTag("GameController");
        managerScript = manager.GetComponent<Manager>();
    }

    public virtual void Initialize(GameObject pointPrefab, bool debug=false)
    {
        List<Vector2> drawPointPositions = customRune;
        List<Vector2> transformedPoints = drawPointPositions;
        transformedPoints = TransformDrawpoints(transformedPoints);

        foreach (Vector3 point in drawPointPositions)
        {
            Vector3 vec3Point = new Vector3(point.x, point.y, 0); // Expected to already be scaled to the [-0.5, 0.5] range.
            GameObject newSphere = Instantiate(pointPrefab, transform, false);
            newSphere.transform.localPosition = vec3Point;
            newSphere.transform.localRotation = Quaternion.identity;
            if (debug)
            {
                newSphere.GetComponent<MeshRenderer>().enabled = true;
            }

            drawPoints.Add(newSphere);
        }

        nextPoint = 0;
    }

    public abstract void DoSpell();

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

    List<Vector2> TransformDrawpoints(List<Vector2> points, float xsize=1f, float ysize=1f, float xoffset=0f, float yoffset=0f)
    {
        List<Vector2> newPoints = points;

        Vector2 xysize = new Vector2(xsize, ysize);
        Vector2 xyoffset = new Vector2(xoffset, yoffset);

        for (int i = 0; i < newPoints.Count; i++)
        {

            newPoints[i] -= new Vector2(0.5f, 0.5f); // Change from [0, 1] to [-0.5, 0.5]
            newPoints[i].Scale(new Vector2(-1, 1)); // Flip direction for x
            newPoints[i].Scale(xysize);
            newPoints[i] += xyoffset;
        }

        return newPoints;
    }
}
