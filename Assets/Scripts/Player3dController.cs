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

    public GameObject attackVFX; // Kéo hiệu ứng VFX vào đây từ Inspector
    public float vfxDuration = 2f; // Thời gian hiển thị của VFX
    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        // Đảm bảo rằng VFX ban đầu là tắt đi
        if (attackVFX != null)
        {
            attackVFX.SetActive(false);
        }
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
            // Hiển thị VFX
            if (attackVFX != null)
            {
                attackVFX.SetActive(true);
                // Khởi động Coroutine để tắt VFX sau một khoảng thời gian
                StartCoroutine(TurnOffVFXAfterDuration());
            }

        }
         
    }
    // Coroutine để tắt VFX sau một khoảng thời gian
    private IEnumerator TurnOffVFXAfterDuration()
    {
        // Chờ một khoảng thời gian nhất định
        yield return new WaitForSeconds(vfxDuration);
        // Tắt VFX
        if (attackVFX != null)
        {
            attackVFX.SetActive(false);
        }
    }
}
