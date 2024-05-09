using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player3dController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    public Animator animator;
    //CharacterController characterController;
    // Start is called before the first frame update
    void Start()
    {
        //animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        float moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        var moveInput = (new Vector3(horizontalInput, 0, verticalInput)).normalized;

        transform.position += moveInput * moveSpeed * Time.deltaTime;
        animator.SetFloat("Speed", moveAmount, 0.25f, Time.deltaTime);
    }
}
