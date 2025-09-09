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

    private Rigidbody rb;
    private Manager managerScript;

    private void Start()
    {
        //rb = nib.GetComponent<Rigidbody>();
        GameObject manager = GameObject.FindGameObjectWithTag("GameController");
        managerScript = manager.GetComponent<Manager>();
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
                currentTarget = targetMousePoint;
            } else
            {
                currentTarget = targetMousePoint - transform.forward * 0.08f + transform.right * 0.08f;
            }

            Quaternion rotation = Quaternion.LookRotation(currentTarget - transform.position);

            nib.transform.SetPositionAndRotation(currentTarget, rotation);
        } else
        {
            Vector3 worldResetPoint = transform.TransformPoint(resetPoint);

            nib.transform.SetPositionAndRotation(worldResetPoint, transform.rotation);
        }
    }
}
