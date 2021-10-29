using UnityEngine;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 2f;

    public UnityEvent OnFPress;

    private CharacterController charController;

    private Vector3 physicsAcceleration = new Vector3();
    private Vector3 physicsVelocity = new Vector3();
    private Vector3 physicsMovement = new Vector3();

    private Vector3 controlledMovement = new Vector3();

    [Header("In Air Detection")]
    public bool canAirJump;

    public bool hasLanded;

    private bool hasJumped = false;

    public Vector3 footPos = new Vector3(0, -1, 0);
    public float footChunkiness = 0.5f;

    public LayerMask inAirCheckLayerMask;

    private PlayerInput playerInput;

    private void Awake()
    {
        playerInput = new PlayerInput();
    }

    private void Start()
    {
        charController = gameObject.GetComponent<CharacterController>();
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
        if (hasJumped)
        {
            hasLanded = Physics.OverlapSphere(transform.position + footPos, footChunkiness, 1 << inAirCheckLayerMask).Length != 0;
            physicsVelocity += physicsAcceleration + Physics.gravity * Time.fixedDeltaTime;
            if (hasLanded)
            {
                hasJumped = false;
                physicsVelocity.y = 0;
            }
        }
        else
        {
            physicsVelocity += physicsAcceleration * Time.fixedDeltaTime;
        }

        physicsMovement += physicsVelocity * Time.fixedDeltaTime;

        charController.Move(physicsMovement);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            OnFPress.Invoke();
        }

        controlledMovement = transform.forward * playerInput.InGame.Move.ReadValue<Vector2>().y + transform.right * playerInput.InGame.Move.ReadValue<Vector2>().x;

        charController.Move(controlledMovement * moveSpeed * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Color foot = Color.blue;
        foot.a = 0.4f;
        Gizmos.color = foot;
        Gizmos.DrawSphere(transform.position + footPos, footChunkiness);
    }

    private void TryJump()
    {
        if (hasJumped)
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
        hasJumped = true;
        hasLanded = false;
        }
        charController.Move(new Vector3(0, 3, 0));
    }

    private void EnableInput()
    {
        playerInput.InGame.Jump.performed += ctx => TryJump();
        playerInput.InGame.Jump.Enable();
        playerInput.InGame.Move.Enable();
    }

    private void DisableInput()
    {
        playerInput.InGame.Jump.Disable();
        playerInput.InGame.Move.Disable();
    }
}