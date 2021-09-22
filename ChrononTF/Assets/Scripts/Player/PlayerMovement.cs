using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 2f;

    public UnityEvent OnFPress;

    CharacterController charController;

    private Vector3 velocity = new Vector3();

    [Header("In Air Detection")]

    public bool canAirJump;

    public bool isInAir;

    public Vector3 footPos = new Vector3(0, -1, 0);
    public float footChunkiness = 0.5f;

    public LayerMask inAirCheckLayerMask;

    // Start is called before the first frame update
    void Start()
    {
        charController = gameObject.GetComponent<CharacterController>();
    }

    private void FixedUpdate()
    {
        isInAir = Physics.OverlapSphere(transform.position + footPos, footChunkiness, 1 << inAirCheckLayerMask).Length == 0;
        if (isInAir)
        {
            velocity += Vector3.down * 9.8f * Time.deltaTime;
        }
        else
        {
            velocity.y = 0;
        }

        charController.Move(velocity);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            OnFPress.Invoke();
        }

        charController.Move((transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal")).normalized * moveSpeed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isInAir)
            {
                if (canAirJump)
                {
                    //Jump
                }
            }
            else
            {
                //Jump
            }
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            charController.Move(new Vector3(0, 3, 0));
        }
    }

    private void OnDrawGizmos()
    {
        Color foot = Color.blue;
        foot.a = 0.4f;
        Gizmos.color = foot;
        Gizmos.DrawSphere(transform.position + footPos, footChunkiness);
    }
}
