using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent (typeof(PlayerAudio))]
public class CharacterMovement : MonoBehaviour
{
    CharacterController cc;
    public Vector3 movedir;
    InputAction moveAction;
    InputAction jumpAction;

    public float speed = 0.3f;
    public float airManoeuvrability = 1f;
    public float jumpSpeed = 1f;

    public float friction = 0.1f;

    public float speedBuff = 1f;
    public float jumpBuff = 1f;

    private float currentJumpTime;
    private Vector3 vectorFriction;

    private PlayerAudio playerAudioScript;
    public float timeBetweenSteps;
    private float currentStepTime;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        playerAudioScript = GetComponent<PlayerAudio>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movedir = Vector3.zero;
        vectorFriction = new Vector3(friction, 0, friction);
        currentStepTime = 0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Movement
        Vector2 moveInput = moveAction.ReadValue<Vector2>();

        Vector3 moveInputVector = new Vector3(moveInput.x, 0, moveInput.y) * speed * speedBuff;

        Quaternion rotation = Quaternion.FromToRotation(Vector3.forward, transform.forward);
        moveInputVector = rotation * moveInputVector;

        if (!cc.isGrounded) {
            movedir += Physics.gravity * Time.deltaTime; //Need an extra deltatime to convert accel to velocity.
            //movedir += ClampxzMagnitude(moveInputVector - Vector3.Project(moveInputVector, new Vector3(cc.velocity.x, 0, cc.velocity.z).normalized), airManoeuvrability * Time.deltaTime);
            movedir += ClampxzMagnitude(moveInputVector, airManoeuvrability * Time.deltaTime);
        } else
        {
            movedir.Scale(Vector3.one - vectorFriction);
            currentJumpTime = 0.2f;
            movedir += moveInputVector;
            if (currentStepTime > timeBetweenSteps / speedBuff){
                playerAudioScript.PlayStep();
            }
        }

        if (currentJumpTime > 0)
        {
            if (jumpAction.WasPressedThisFrame())
            {
                playerAudioScript.PlayJump();
            }

            if (jumpAction.IsPressed())
            {
                movedir += Vector3.up * jumpSpeed * jumpBuff;

                if (cc.velocity.y < 1.5f * jumpBuff)
                {
                    movedir += Vector3.up * 1.5f * jumpBuff;
                }

                currentJumpTime -= Time.deltaTime;
            } else
            {
                currentJumpTime = 0;
            }
        }

        if (cc.isGrounded)
        {
            movedir = ClampxzMagnitude(movedir, 9.3f * speedBuff);
        } else
        {
            Vector3 clamped = ClampxzMagnitude(movedir, 1.3f * 9.3f * speedBuff);
            movedir = new Vector3(clamped.x, movedir.y, clamped.z);
        }

        cc.Move(movedir * Time.deltaTime);

        currentStepTime += Time.deltaTime;
    }

    Vector3 ClampxzMagnitude(Vector3 inputVector, float max)
    {
        float xzMagnitude = inputVector.magnitude;
        xzMagnitude = Mathf.Clamp(xzMagnitude, 0, max);
        return inputVector.normalized * xzMagnitude;
    }
}
