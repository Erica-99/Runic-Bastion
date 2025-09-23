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

    private void Start()
    {
        GameObject manager = GameObject.FindGameObjectWithTag("GameController");
        managerScript = manager.GetComponent<Manager>();
        nibCollider = nib.GetComponent<Collider>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
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
        } else
        {
            nibCollider.enabled = false;

            Vector3 worldResetPoint = transform.TransformPoint(resetPoint);
            nib.transform.SetPositionAndRotation(worldResetPoint, transform.rotation);
        }
    }
}
