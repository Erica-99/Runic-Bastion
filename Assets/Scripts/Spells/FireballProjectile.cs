using UnityEngine;
using UnityEngine.InputSystem;

public class FireballProjectile : MonoBehaviour, ICastable
{
    GameObject characterObject;

    public bool readied { get; set; }
    public bool casted { get; set; }

    public float damageBuff { get; set; }

    public GameObject vfxObject;
    public GameObject explosionObject;

    public float speed = 1f;

    public float maxExplosionSize = 15f;

    public float damage = 7f;

    public LayerMask aimLayerMask;

    private Rigidbody rb;

    private InputAction castInput;

    private bool popped = false;

    void OnEnable()
    {
        castInput.performed += context => ReceiveCastInput(context);
    }

    void OnDisable()
    {
        castInput.performed -= context => ReceiveCastInput(context);
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        castInput = InputSystem.actions.FindAction("Attack");
    }


    // Put the fireball in your hand
    public void ReadyCast(GameObject character)
    {
        if (readied == true)
        {
            return;
        }

        characterObject = character;

        transform.parent = characterObject.GetComponent<AttackManager>().playerCamera.transform;

        transform.localPosition = new Vector3(-0.7f, -0.15f, 0.468f);
        transform.localRotation = Quaternion.identity;

        readied = true;
    }

    // Shoot fireball
    public void DoCast()
    {
        if (!readied || casted)
        {
            return;
        }

        rb.isKinematic = false;

        Camera mainCam = Camera.main;

        Ray aimRay = mainCam.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 100f));
        RaycastHit hit;
        Vector3 aimPoint;

        if (!Physics.Raycast(aimRay, out hit, maxDistance: 100f, layerMask: aimLayerMask))
        {
            aimPoint = mainCam.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, 100f));
        } else
        {
            aimPoint = hit.point;
        }

        Vector3 forceDirection = (aimPoint - transform.position).normalized;

        transform.parent = null;
        
        rb.AddForce(forceDirection * speed, ForceMode.Impulse);

        vfxObject.GetComponent<Collider>().enabled = true;

        readied = false;
        casted = true;
    }

    private void ReceiveCastInput(InputAction.CallbackContext context)
    {
        DoCast();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!popped)
        {
            rb.isKinematic = true;
            
            vfxObject.SetActive(false);
            explosionObject.SetActive(true);
            popped = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            EnemyMovement enemyScript = other.gameObject.GetComponent<EnemyMovement>();

            enemyScript.TakeDamage(damage * damageBuff);
        }
    }

    private void FixedUpdate()
    {
        if (!casted)
        {
            transform.localScale = new Vector3(damageBuff, damageBuff, damageBuff);
        }

        if (!popped)
        {
            return;
        }

        if (explosionObject.transform.localScale.x >= maxExplosionSize)
        {
            Destroy(gameObject);
        } else
        {
            float scaleUp = maxExplosionSize * Time.deltaTime / 0.7f;

            explosionObject.transform.localScale += new Vector3(scaleUp, scaleUp, scaleUp);
        }  
    }
}
