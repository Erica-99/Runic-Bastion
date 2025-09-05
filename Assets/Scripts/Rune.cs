using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;
using UnityEngine;

public class Rune : MonoBehaviour
{


    private List<GameObject> drawPoints;
    private GameObject nextPoint;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void Initialize(List<Vector3> drawPointPositions, GameObject pointPrefab, bool debug=false)
    {
        foreach (Vector3 point in drawPointPositions)
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
