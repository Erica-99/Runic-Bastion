using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class VineBehaviour : MonoBehaviour
{
    private bool released = false;
    private bool triggered = false;
    private bool startDescent = false;

    private GameObject target;

    public GameObject curvy_model;
    public GameObject spike_model;
    public float speed;

    private float modelRotationX = 0;
    private float curvyVineDownTotalTime = 1f;
    private float curvyVineDownCurrentTime = 0f;

    private float currVel = 0f;
    private SphereCollider coll;

    public float spikeAnimationTime = 5f;
    private float currentSpikeAnimationPoint;

    public void Release(GameObject _target)
    {
        target = _target;
        coll = GetComponent<SphereCollider>();

        // Set initial heading.
        NavMeshAgent agent = target.GetComponent<NavMeshAgent>();
        Vector3 vectorToAgent = target.transform.position - transform.position;
        float enemySpeedToVine = Vector3.Project(agent.velocity, -vectorToAgent).magnitude;
        float collideTimeEst = vectorToAgent.magnitude / (speed + enemySpeedToVine) + curvyVineDownTotalTime / 2;
        Vector3 enemyPredictedVector = agent.velocity * collideTimeEst;
        Vector3 enemyPredictedPoint = target.transform.position + enemyPredictedVector;
        Vector3 targetPoint = new Vector3(enemyPredictedPoint.x, transform.position.y, enemyPredictedPoint.z);
        transform.rotation = Quaternion.LookRotation(targetPoint - transform.position);

        released = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!released) return;

        if (triggered)
        {
            SpikeVine();
        } else
        {
            MovingVine();
        }
    }

    void MovingVine()
    {
        NavMeshAgent agent = target.GetComponent<NavMeshAgent>();

        Vector3 vectorToAgent = target.transform.position - transform.position;
        float enemySpeedToVine = Vector3.Project(agent.velocity, -vectorToAgent).magnitude;
        float collideTimeEst = vectorToAgent.magnitude / (speed + enemySpeedToVine) + curvyVineDownTotalTime/2;
        Vector3 enemyPredictedVector = agent.velocity * collideTimeEst;
        Vector3 enemyPredictedPoint = target.transform.position + enemyPredictedVector;
        Vector3 targetPoint = new Vector3(enemyPredictedPoint.x, transform.position.y, enemyPredictedPoint.z);
        Vector3 targetToLook = Vector3.Lerp(transform.forward, targetPoint - transform.position, 0.2f*Time.deltaTime);

        transform.rotation = Quaternion.LookRotation(targetToLook);

        currVel = speed * Time.deltaTime;

        transform.position = transform.position + currVel * transform.forward;

        if ((targetPoint-transform.position).magnitude < coll.radius)
        {
            startDescent = true;
            coll.enabled = false;
        }

        MovingVineAnimation(currVel);
    }

    void SpikeVine()
    {
        if (currentSpikeAnimationPoint > spikeAnimationTime)
        {
            Destroy(gameObject);
            return;
        }

        float relativeAnimationProgress = currentSpikeAnimationPoint / spikeAnimationTime;
        float sectionTime = 0f;
        float spikeTargetY = 0f;
        Vector3 basePos = new Vector3();

        if (relativeAnimationProgress < 0.1f)
        {
            basePos = new Vector3(0f, -6f, 0f);
            sectionTime = relativeAnimationProgress / 0.1f;
            spikeTargetY = 0f;
        } else if (relativeAnimationProgress < 0.6f)
        {
            basePos = new Vector3(0f, 0f, 0f);
            sectionTime = (relativeAnimationProgress - 0.2f) / 0.5f;
            spikeTargetY = -0.2f;
        } else
        {
            basePos = new Vector3(0f, -0.2f, 0f);
            sectionTime = (relativeAnimationProgress - 0.6f) / 0.4f;
            spikeTargetY = -8f;
        }

        spike_model.transform.localPosition = Vector3.Lerp(basePos, new Vector3(0f, spikeTargetY, 0f), sectionTime);

        currentSpikeAnimationPoint += Time.deltaTime;
    }

    void MovingVineAnimation(float linearMovement)
    {
        // - Descending -
        if (startDescent)
        {
            curvyVineDownCurrentTime += Time.deltaTime;

            if (curvyVineDownCurrentTime > curvyVineDownTotalTime)
            {
                triggered = true;
                curvy_model.SetActive(false);
                spike_model.SetActive(true);
                return;
            }

            Vector3 newPos = Vector3.Lerp(Vector3.zero, new Vector3(0f, -2f, 0f), curvyVineDownCurrentTime/curvyVineDownTotalTime);

            curvy_model.transform.localPosition = newPos;
        }
        
        // - Rotation -
        //First calculate outer radius.
        float majorRad = 0.9f * curvy_model.transform.localScale.x / 100f;

        //Calculate circumference
        float majorCirc = 2f * Mathf.PI * majorRad;

        //Calculate fraction of circumference traveled linearly
        float circFrac = linearMovement / majorCirc;

        modelRotationX -= circFrac * 360f;

        curvy_model.transform.localRotation = Quaternion.Euler(new Vector3(modelRotationX, 0, 90));
        
    }
}
