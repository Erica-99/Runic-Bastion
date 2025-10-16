using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public Transform target;
    private NavMeshAgent agent;

    private Rigidbody rb;

    public float DistToCrystal
    {
        get
        {
            return Vector3.Distance(transform.position, target.position);
        }
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = true;
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        agent.destination = target.position;
    }
}
