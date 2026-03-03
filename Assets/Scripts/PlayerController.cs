using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private PlayerStateManager stateManager;


    //adjustables
    [SerializeField] float yawLookSensitivity;
    [SerializeField] float pitchLookSensitivity;

    [SerializeField] float maxLookPitch;

    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;
    //

    //movment vectors
    private Vector3 targetDir;

    private float xLookAngle;
    private float yLookAngle;
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

        yLookAngle = Mathf.Clamp(yLookAngle + lookInput.y * pitchLookSensitivity, -maxLookPitch, maxLookPitch);
        xLookAngle -= lookInput.x * yawLookSensitivity;

        rb.MoveRotation(Quaternion.Euler(
            yLookAngle,
            xLookAngle,
            0
            ));
    }

    private void GetReqMoveDir()
    {
        Vector3 adjustedForward = transform.forward;
        

        targetDir = new Vector3(move.ReadValue<Vector2>().x * adjustedForward.x, 0, move.ReadValue<Vector2>().y * adjustedForward.z);

        


        
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
    }
}
