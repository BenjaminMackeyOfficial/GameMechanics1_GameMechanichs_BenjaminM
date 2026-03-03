using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private PlayerStateManager stateManager;
    private GameObject cam;

    //adjustables
    [SerializeField] float yawLookSensitivity;
    [SerializeField] float pitchLookSensitivity;

    [SerializeField] float maxLookPitch;

    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;
    //

    //movment vectors
    private Vector3 targetDir;
    private Quaternion lookRot;

    private float xLookAngle;
    private float yLookAngle;

    private float movement;
    //

    //inputs
    [SerializeField] InputActionAsset inputActions;

    private InputAction move;
    private InputAction jump;
    private InputAction look;
    private InputAction sprint;
    //



    // Start is called once before the first execution of Update after the MonoBehaviour is created


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cam = transform.Find("Camera").gameObject;
        stateManager = GetComponent<PlayerStateManager>();
        if (stateManager == null) stateManager = gameObject.AddComponent<PlayerStateManager>();

        if(inputActions == null) this.enabled = false;

        move = inputActions.FindAction("Player/Move");
        jump = inputActions.FindAction("Player/Jump");
        look = inputActions.FindAction("Player/Look");
        sprint = inputActions.FindAction("Player/Sprint");
    }

    private void rotateForLook()
    {
        Vector2 lookInput = -look.ReadValue<Vector2>();

        yLookAngle = Mathf.Clamp(yLookAngle + lookInput.y * pitchLookSensitivity * Time.deltaTime, -maxLookPitch, maxLookPitch);
        xLookAngle -= lookInput.x * yawLookSensitivity * Time.deltaTime;

        lookRot = Quaternion.Euler(
            yLookAngle,
            xLookAngle,
            0
            );
    }

    private void GetReqMoveDir()
    {
        Vector3 adjustedForward = transform.forward;
        float inputted = move.ReadValue<Vector2>().y;

        Quaternion rot = Quaternion.AngleAxis(xLookAngle, stateManager.groundUp);
        adjustedForward = rot * adjustedForward;
        adjustedForward.Normalize();


        targetDir = adjustedForward * move.ReadValue<Vector2>().y;
    }

    // Update is called once per frame
    void Update()
    {
        rotateForLook();
    }

    void FixedUpdate()
    {
        stateManager.checkGround();


        GetReqMoveDir();

        rb.Move(transform.position + targetDir, Quaternion.identity);
        cam.transform.rotation = lookRot;
    }
}
