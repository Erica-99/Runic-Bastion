using UnityEngine;

public class PenMovement : MonoBehaviour
{
    public bool followCursor;
    public bool mouseDown;
    public Vector3 targetMousePoint;

    [SerializeField]
    private GameObject nib;

    [SerializeField]
    private Vector3 resetPoint;

    private Collider nibCollider;
    private Manager managerScript;

    //private Rigidbody nibRB;

    private void Start()
    {
        GameObject manager = GameObject.FindGameObjectWithTag("GameController");
        managerScript = manager.GetComponent<Manager>();
        nibCollider = nib.GetComponent<Collider>();

        //nibRB = nib.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Physics.SyncTransforms();

        if (followCursor)
        {
            Vector3 currentTarget;
            if (mouseDown)
            {
                if (!nibCollider.enabled) nibCollider.enabled = true;
                currentTarget = targetMousePoint;

            } else
            {
                if (nibCollider.enabled) nibCollider.enabled = false;
                currentTarget = targetMousePoint - transform.forward * 0.08f + transform.right * 0.08f;
            }

            Quaternion rotation = Quaternion.LookRotation(currentTarget - transform.position);


            nib.transform.SetPositionAndRotation(currentTarget, rotation);
            //nibRB.Move(currentTarget, rotation);
        } else
        {
            nibCollider.enabled = false;

            Vector3 worldResetPoint = transform.TransformPoint(resetPoint);
            //nibRB.Move(worldResetPoint, transform.rotation);
            nib.transform.SetPositionAndRotation(worldResetPoint, transform.rotation);
        }
    }
}
