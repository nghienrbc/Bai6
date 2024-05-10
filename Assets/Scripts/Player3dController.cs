using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player3dController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float rotationSpeed = 5f;

    public Animator animator;
    //CharacterController characterController;
    // Start is called before the first frame update
    void Start()
    { 
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        float moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        var moveDirection = (new Vector3(horizontalInput, 0, verticalInput)).normalized;


        if (Input.GetKey(KeyCode.LeftShift) && moveDirection != Vector3.zero)
        {
            moveAmount += 1;
            transform.position += moveDirection * runSpeed * Time.deltaTime;
        }
        else transform.position += moveDirection * moveSpeed * Time.deltaTime;
        animator.SetFloat("Speed", moveAmount);

        if (moveDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed*Time.deltaTime); 
        } 
    } 
}
