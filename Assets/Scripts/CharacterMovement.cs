using System;
using System.Runtime.CompilerServices;
using TreeEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    CharacterController cc;
    Vector3 movedir;
    InputAction moveAction;
    InputAction jumpAction;

    public float speed = 0.3f;
    public float airManoeuvrability = 1f;
    public float jumpSpeed = 1f;

    public float friction = 0.1f;

    private float currentJumpTime;
    private Vector3 vectorFriction;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movedir = Vector3.zero;
        vectorFriction = new Vector3(friction, 0, friction);
    }

    // Update is called once per frame
    void Update()
    {
        // Movement
        Vector2 moveInput = moveAction.ReadValue<Vector2>();

        Vector3 moveInputVector = new Vector3(moveInput.x, 0, moveInput.y) * speed;

        Quaternion rotation = Quaternion.FromToRotation(Vector3.forward, transform.forward);
        moveInputVector = rotation * moveInputVector;

        if (!cc.isGrounded) {
            movedir += Physics.gravity * Time.deltaTime; //Need an extra deltatime to convert accel to velocity.
            movedir += ClampxzMagnitude(moveInputVector - Vector3.Project(moveInputVector, new Vector3(cc.velocity.x, 0, cc.velocity.z).normalized), airManoeuvrability * Time.deltaTime);
        } else
        {
            movedir.Scale(Vector3.one - vectorFriction);
            currentJumpTime = 0.2f;
            movedir += moveInputVector;
        }

        if (currentJumpTime > 0)
        {
            if (jumpAction.IsPressed())
            {
                movedir += Vector3.up * jumpSpeed;

                if (cc.velocity.y < 1.5f)
                {
                    movedir += Vector3.up * 1.5f;
                }

                currentJumpTime -= Time.deltaTime;
            } else
            {
                currentJumpTime = 0;
            }
        }

        cc.Move(movedir * Time.deltaTime);
    }

    Vector3 ClampxzMagnitude(Vector3 inputVector, float max)
    {
        float xzMagnitude = inputVector.magnitude;
        xzMagnitude = Mathf.Clamp(xzMagnitude, 0, max);
        return inputVector.normalized * xzMagnitude;
    }
}
