using UnityEngine;
using UnityEngine.InputSystem;

public class PlaceableTower : MonoBehaviour, ICastable
{
    public bool readied { get; set; }
    public bool casted { get; set; }
    public float damageBuff { get; set; }

    private InputAction castInput;

    GameObject characterObject;

    public GameObject placementIndicator;
    public LayerMask aimLayerMask;

    private Vector3 targetPoint;
    private Camera mainCam;
    private Vector3 screenCentre;

    private GameObject indicator;
    private Renderer indicatorRenderer;
    public float placeableRadius;

    private bool enablePlace = false;

    private int ignoreRaycastLayermask;

    public void ReadyCast(GameObject character)
    {
        if (readied == true)
        {
            return;
        }

        characterObject = character;

        transform.parent = characterObject.GetComponent<AttackManager>().playerCamera.transform;

        transform.localPosition = new Vector3(-0.47f, -0.79f, 0.47f);
        transform.localRotation = Quaternion.identity;
        transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        readied = true;

        indicator = Instantiate(placementIndicator);
        indicatorRenderer = indicator.GetComponent<Renderer>();
    }

    public void DoCast()
    {

    }

    void Awake()
    {
        castInput = InputSystem.actions.FindAction("Attack");

        mainCam = Camera.main;
        screenCentre = new Vector3(Screen.width / 2f, Screen.height / 2f, 100f);

        int ignoreRaycastLayerIndex = LayerMask.NameToLayer("Ignore Raycast");
        ignoreRaycastLayermask = 1 << ignoreRaycastLayerIndex;
    }

    void FixedUpdate()
    {
        Ray aimRay = mainCam.ScreenPointToRay(screenCentre);
        RaycastHit hit;
        bool hitHedge = false;

        if (!Physics.Raycast(aimRay, out hit, maxDistance: placeableRadius, layerMask: aimLayerMask))
        {
            Ray secondAimRay = new Ray(aimRay.GetPoint(placeableRadius), Vector3.down);
            RaycastHit secondHit;

            if (!Physics.Raycast(secondAimRay, out secondHit, maxDistance: 100f, layerMask: aimLayerMask))
            {
                targetPoint = aimRay.GetPoint(placeableRadius);
            } else
            {
                targetPoint = secondHit.point;

                if (secondHit.collider.gameObject.CompareTag("Hedge"))
                {
                    hitHedge = true;
                }
            }
        }
        else
        {
            targetPoint = hit.point;
            
            if (hit.collider.gameObject.CompareTag("Hedge"))
            {
                hitHedge = true;
            }
        }

        indicator.transform.position = targetPoint;

        if (hitHedge)
        {
            Ray hedgetopRay = new Ray(targetPoint, Vector3.down);
            RaycastHit hedgetopHit;

            if (!Physics.Raycast(hedgetopRay, out hedgetopHit, maxDistance: 1f, layerMask: ignoreRaycastLayermask))
            {
                indicatorRenderer.material.color = new Color(0.8313726f, 0.2333552f, 0f, 0.5882353f); //Red
                enablePlace = false;
            } else
            {
                indicatorRenderer.material.color = new Color(0f, 0.6917208f, 0.8322851f, 0.5882353f); //Blue
                enablePlace = true;
            }
                
        } else
        {
            indicatorRenderer.material.color = new Color(0.8313726f, 0.2333552f, 0f, 0.5882353f); //Red
            enablePlace = false;
        }

    }

    void OnEnable()
    {
        castInput.performed += context => ReceiveCastInput(context);
    }

    void OnDisable()
    {
        castInput.performed -= context => ReceiveCastInput(context);
    }

    private void ReceiveCastInput(InputAction.CallbackContext context)
    {
        DoCast();
    }
}
