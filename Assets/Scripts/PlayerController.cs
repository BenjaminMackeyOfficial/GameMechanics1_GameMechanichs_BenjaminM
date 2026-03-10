using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private PlayerStateManager stateManager;
    private GameObject cam;

    //adjustables
    [SerializeField] float yawLookSensitivity;
    [SerializeField] float pitchLookSensitivity;

    [SerializeField] float maxLookPitch;

    [SerializeField] float maxSlopeAngle;
    [SerializeField] float walkSpeed;
    [SerializeField] float jumpHeight;
    [SerializeField] float downForceWhileInAir;

    [SerializeField] float Bouncyness;
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

    
    //capsule colider stuff
    private Vector3 heightFromCent;
    //

    [SerializeField] GameObject[] objs;
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

        heightFromCent =  Vector3.up * (transform.localScale.y / 4);

        jump.started += Jump;
        jump.canceled += Jump;

        Cursor.lockState = CursorLockMode.Locked;
    }
    float jumpin = 0f;
    private void Jump(InputAction.CallbackContext inputAction)
    {
        if(inputAction.ReadValue<float>() > 0f)
        {
            jumpin += jumpHeight * 10f;
        }
        else
        {
            jumpin = 0;
        }
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
        Vector3 adjustedForward;
        Vector3 adjustedRight;

        float inputted = move.ReadValue<Vector2>().y;

        Quaternion rot = Quaternion.AngleAxis(xLookAngle, stateManager.groundUp);
        Quaternion rot2 = Quaternion.AngleAxis(xLookAngle + 90f, stateManager.groundUp);
        adjustedForward = rot * transform.forward;
        adjustedRight = rot2 * transform.forward;

        adjustedForward.Normalize();
        adjustedRight.Normalize();

        targetDir = (adjustedForward * move.ReadValue<Vector2>().y) + 
        (adjustedRight * move.ReadValue<Vector2>().x);
        targetDir.Normalize();
    }


    //outside vars for memory assignment
    Vector3 sphere1 = Vector3.zero;
    Vector3 sphere2 = Vector3.zero;
    private bool CheckForCollisions(Vector3 dir, out RaycastHit hit)
    {
        sphere1 = transform.position + heightFromCent;
        sphere2 = transform.position - heightFromCent;

        return Physics.CapsuleCast(sphere1 , sphere2 , 0.5f, dir, out hit, walkSpeed * Time.deltaTime);
    }
    private bool CheckForCollisions(Vector3 dir)
    {
        sphere1 = transform.position + heightFromCent;
        sphere2 = transform.position - heightFromCent;

        return Physics.CapsuleCast(sphere1 , sphere2 , 0.5f, dir,walkSpeed * Time.deltaTime);
    }
    private Vector3 AdjustMoveDir(RaycastHit hit)
    {
        Vector3 returnVec = Vector3.Reflect(targetDir, hit.normal);

        float speedLoss = Mathf.Abs((returnVec - targetDir).magnitude) * Bouncyness;

        returnVec *= speedLoss;
        //if(CheckForCollisions(returnVec)) return Vector3.zero;
        return returnVec;
    }


    // Update is called once per frame
    void Update()
    {
        rotateForLook();
        cam.transform.rotation = lookRot;
    }


    void FixedUpdate()
    {
        Vector3 ground= stateManager.checkGround(maxSlopeAngle);

        GetReqMoveDir();

        Vector3 newMovePos;
        float velClamper = walkSpeed * Mathf.Clamp01(1f - (rb.linearVelocity.magnitude / walkSpeed));
        if(stateManager.grounded) 
        {
            newMovePos = (targetDir * Time.deltaTime * velClamper * 10000f);

            rb.AddForce(Vector3.up * jumpin, ForceMode.Impulse); // jumping
            rb.AddForce(newMovePos, ForceMode.Force); //moving 
            
            rb.linearVelocity = new Vector3(rb.linearVelocity.x * 0.95f, rb.linearVelocity.y, rb.linearVelocity.z *0.95f);
        }
        else 
        {
            rb.AddForce(Vector3.down * downForceWhileInAir * 100f);
        }
        

        
    }

    public void Teleport(Vector3 pos)
    {
        if(Physics.OverlapCapsule(
                pos + Vector3.up * 0.5f,
                pos - Vector3.up * 0.5f,
                0.5f, ~(1 << 2)).Length ==0 )
        {
            transform.position = pos;
        }
    }
}
