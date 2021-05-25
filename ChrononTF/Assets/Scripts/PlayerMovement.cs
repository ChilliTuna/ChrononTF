using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 0.1f;

    public UnityEvent OnFPress;

    CharacterController charController;

    // Start is called before the first frame update
    void Start()
    {
        charController = gameObject.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            OnFPress.Invoke();
        }

        if (Input.GetKey(KeyCode.W))
        {
            charController.Move(transform.forward * moveSpeed);
        }
        if (Input.GetKey(KeyCode.A))
        {
            charController.Move(transform.right * -moveSpeed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            charController.Move(transform.forward * -moveSpeed);
        }
        if (Input.GetKey(KeyCode.D))
        {
            charController.Move(transform.right * moveSpeed);
        }
    }
}
