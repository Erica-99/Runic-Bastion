using UnityEngine;

public class VineBehaviour : MonoBehaviour
{
    private bool triggered = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (triggered)
        {
            MovingVine();
        } else
        {
            SpikeVine();
        }
    }

    void MovingVine()
    {

    }

    void SpikeVine()
    {

    }
}
