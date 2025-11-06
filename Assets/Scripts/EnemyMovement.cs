using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public float health = 10f;
    public int damage = 1;
    public float attackTime = 1f;

    public Transform target;
    private NavMeshAgent agent;
    private Rigidbody rb;

    private float lastAttackTime = 0f;

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

    public bool TakeDamage(float amount)
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

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Crystal")
        {
            if (PlayerLives.Lives > 0 && Time.time - lastAttackTime >= attackTime)
            {
                lastAttackTime = Time.time;
                PlayerLives.Lives -= damage;
                print($"Player lives left: {PlayerLives.Lives}");
            }
        }
    }

    void Die()
    {
        WaveSpawner.enemiesAlive--;
        Destroy(gameObject);
    }
}
