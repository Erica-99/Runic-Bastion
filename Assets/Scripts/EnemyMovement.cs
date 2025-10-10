using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public Transform target;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        agent.destination = target.position;
    }

    public class CollisionDetector : MonoBehaviour
    {
        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag.Equals("Crystal") == true)
            {
                Destroy(gameObject);
            }
        }
    }

    }
