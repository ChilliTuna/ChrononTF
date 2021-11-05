using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Variables

    [Header("Movement")]
    public float moveSpeed = 2f;
    public Vector3 controlledMovement = new Vector3();
    public float jumpHeight = 0.3f;

    [Header("Player Specific Physics")]
    [SerializeField]
    private Vector3 physicsAcceleration = new Vector3();

    [SerializeField]
    private Vector3 physicsVelocity = new Vector3();

    [SerializeField]
    private Vector3 physicsMovement = new Vector3();

    [SerializeField]
    private Vector3 personalGravity = new Vector3(0, -9.8f, 0);

    [Header("In Air Detection")]
    public bool canAirJump;
    private bool doGroundChecks = true;
    private bool isGrounded = true;
    public Vector3 groundCheckSize = new Vector3(0.25f, 0.1f, 0.25f);
    public Vector3 groundCheckPos = new Vector3(0, -1, 0);
    public LayerMask groundCheckIgnoreLayerMask;

    [Header("Misc")]
    private PlayerInput playerInput;
    private CharacterController charController;
    private TimeManager timeManager;

    #endregion Variables

    #region Unity Functions

    private void Awake()
    {
        playerInput = new PlayerInput();
    }

    private void Start()
    {
        charController = gameObject.GetComponent<CharacterController>();
        timeManager = FindObjectOfType<TimeManager>();
    }

    private void OnEnable()
    {
        EnableInput();
    }

    private void OnDisable()
    {
        DisableInput();
    }

    private void FixedUpdate()
    {
        if (doGroundChecks)
        {
            isGrounded = Physics.OverlapBox(transform.position + groundCheckPos, groundCheckSize, transform.rotation, 1 << groundCheckIgnoreLayerMask).Length != 0;
            if (isGrounded)
            {
                physicsVelocity.y = 0;
                physicsMovement.y = 0;
            }
            else
            {
                physicsVelocity += physicsAcceleration + personalGravity * Time.fixedDeltaTime;
            }
        }
        else
        {
            physicsVelocity += physicsAcceleration * Time.fixedDeltaTime;
        }

        physicsMovement += physicsVelocity * Time.fixedDeltaTime;

        charController.Move(physicsMovement);

        controlledMovement = transform.forward * playerInput.InGame.Move.ReadValue<Vector2>().y + transform.right * playerInput.InGame.Move.ReadValue<Vector2>().x;

        charController.Move(controlledMovement * moveSpeed * Time.fixedDeltaTime);
    }

    private void OnDrawGizmos()
    {
        Color foot = Color.blue;
        foot.a = 0.4f;
        Gizmos.color = foot;
        Gizmos.DrawCube(transform.position + groundCheckPos, groundCheckSize * 2);
    }

    #endregion Unity Functions

    #region Custom Functions

    #region Jumping

    private void TryJump()
    {
        if (!isGrounded)
        {
            if (canAirJump)
            {
                PerformJump(true);
            }
        }
        else
        {
            PerformJump();
        }
    }

    private void PerformJump(bool doAirJump = false)
    {
        if (!doAirJump)
        {
            doGroundChecks = false;
            isGrounded = false;
        }
        StartCoroutine(DoJumpMove());
    }

    //private void DoJumpMove()
    //{
    //    physicsVelocity.y = Mathf.Sqrt(jumpHeight * -2f * personalGravity.y);
    //}

    private IEnumerator DoJumpMove()
    {
        //u = (s-0.5at^2)/t
        //float upwardsVelocity = (height - (0.5f * personalGravity.y * Mathf.Pow(timeToPeak, 2))) / timeToPeak;
        //physicsVelocity.y += upwardsVelocity;
        physicsVelocity.y = Mathf.Sqrt(jumpHeight * -1f * ( personalGravity.y + physicsAcceleration.y));
        yield return new WaitForSecondsRealtime(0.1f);

        doGroundChecks = true;
    }

    #endregion Jumping

    #region Input Functions

    private void EnableInput()
    {
        playerInput.InGame.StopTime.performed += ctx => timeManager.SwapTimeFlux();
        playerInput.InGame.StopTime.Enable();
        playerInput.InGame.Jump.performed += ctx => TryJump();
        playerInput.InGame.Jump.Enable();
        playerInput.InGame.Move.Enable();
    }

    private void DisableInput()
    {
        playerInput.InGame.Jump.Disable();
        playerInput.InGame.Move.Disable();
    }

    #endregion Input Functions

    #endregion Custom Functions
}