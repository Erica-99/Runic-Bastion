using UnityEngine;
using UnityEngine.InputSystem;

public class FireballProjectile : MonoBehaviour, ICastable
{
    GameObject characterObject;

    public bool readied { get; set; }
    public bool casted { get; set; }

    public GameObject vfxObject;
    public GameObject explosionObject;

    public float speed = 1f;

    public float maxExplosionSize = 15f;

    public LayerMask aimLayerMask;

    private Rigidbody rb;

    private InputAction castInput;

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


    public void ReadyCast(GameObject character)
    {
        if (readied == true)
        {
            return;
        }

        // To be called by attack manager. Set up floating fireball in hand or whatever.
        characterObject = character;

        transform.parent = characterObject.transform;

        transform.localPosition = new Vector3(-0.7f, 0.85f, 0.468f);
        transform.localRotation = Quaternion.identity;

        readied = true;
    }

    public void DoCast()
    {
        if (!readied || casted)
        {
            return;
        }

        // Shoot the fireball forwards.
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

        readied = false;
        casted = true;
    }

    void ReceiveCastInput(InputAction.CallbackContext context)
    {
        DoCast();
    }
}
