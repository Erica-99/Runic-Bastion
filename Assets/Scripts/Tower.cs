using UnityEngine;

public abstract class Tower : MonoBehaviour
{
    protected abstract float Interval { get; }
    private float timeElapsed = 0;
    

    // Update is called once per frame
    void Update()
    {
        DoTimer();
    }

    private void DoTimer()
    {
        if (timeElapsed > Interval)
        {
            timeElapsed = 0;
            Behaviour();
        }
        else
        {
            timeElapsed += Time.deltaTime;
        }
    }

    protected abstract void Behaviour();
}
