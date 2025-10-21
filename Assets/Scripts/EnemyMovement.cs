using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public int health = 10;

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

    public bool TakeDamage(int amount)
    {
        health -= amount;

        if (health <= 0)
        {
            Die();
            return true;
        }

        return false;
    }

    private void Update()
    {
        agent.destination = target.position;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Crystal")
        {
            PlayerLives.Lives--;
            Destroy(gameObject, 3);
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
