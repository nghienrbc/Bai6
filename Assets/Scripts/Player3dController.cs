using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player3dController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float rotationSpeed = 5f;

    public float jumpSpeed = 5f;
    float verlocityY;

    public Animator animator;
    CharacterController characterController;
    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        var moveDirection = (new Vector3(horizontalInput, 0, verticalInput)).normalized;

        float moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        float characterMoveSpeed = moveSpeed;

        if (Input.GetKey(KeyCode.LeftShift) && moveDirection != Vector3.zero)
        {
            moveAmount += 1;
            characterMoveSpeed = runSpeed;
        }

        verlocityY += Physics.gravity.y * Time.deltaTime;
        if (characterController.isGrounded)
        {
            verlocityY = -0.2f;
            if (Input.GetButtonDown("Jump"))
            {
            verlocityY = jumpSpeed;
            }
        }

        Vector3 velocity = moveDirection * moveAmount * characterMoveSpeed;
        velocity.y = verlocityY;

        characterController.Move(velocity * Time.deltaTime);
        animator.SetFloat("Speed", moveAmount, 0.05f, Time.deltaTime);

        if (moveDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed*Time.deltaTime); 
        }

        if (Input.GetButtonDown("Fire1"))
        {
            animator.SetTrigger("Attack");
        }
         
    } 
}
