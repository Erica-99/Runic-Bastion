using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraBehaviour : MonoBehaviour
{
    public GameObject player;

    public float sensitivity;
    private InputAction lookAction;
    private float pitch;
    private float yaw;

    [SerializeField]
    private GameObject manager;
    private Manager managerScript;
    private WaveSpawner waveSpawner;

    //private Rigidbody rb;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        lookAction = InputSystem.actions.FindAction("Look");

        managerScript = manager.GetComponent<Manager>();
        waveSpawner = manager.GetComponent<WaveSpawner>();

        //rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Physics.SyncTransforms();

        if (!(managerScript.castMode || waveSpawner.gameEnded))
        {
            Vector2 lookInput = lookAction.ReadValue<Vector2>() * Time.smoothDeltaTime;

            pitch += lookInput.y * sensitivity;
            yaw += lookInput.x * sensitivity;

            pitch = Mathf.Clamp(pitch, -89, 89);

            float phi = pitch * Mathf.Deg2Rad;
            float theta = yaw * Mathf.Deg2Rad;

            float sinTheta = Mathf.Sin(theta);
            float cosTheta = Mathf.Cos(theta);
            float sinPhi = Mathf.Sin(phi);
            float cosPhi = Mathf.Cos(phi);

            Vector3 fwd = new Vector3(cosPhi * sinTheta, sinPhi, cosPhi * cosTheta);

            Quaternion qrot = Quaternion.LookRotation(fwd, Vector3.up);
            
            //rb.MoveRotation(qrot);

            player.transform.forward = new Vector3(fwd.x, 0, fwd.z);
            transform.forward = fwd;

        }

        // Sync movement to player
        //transform.position = player.transform.position + new Vector3(0, 1f, 0);

        //rb.MovePosition(player.transform.position + new Vector3(0, 1f, 0));
    }
}
