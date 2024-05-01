using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    public float radius;
    Rigidbody2D rb;
    Animator animator;
    private bool m_FacingRight = true;
    float horizontalInput;
    float verticalInput; 
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask layerMaskGround; 
    private bool isGrounded =false; // kiem tra xem player co dang dung tren ground hay khong
    private bool isWasGrounded = false;
    private bool isAttacking = false;
    private bool isPressJumpButton = false;

    private void OnDrawGizmos() // vẽ vòng tròn từ tâm của vị trí groundcheck trên player, tương ứng với Physics2D.OverlapCircle
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.transform.position, radius);
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        if (Input.GetKeyDown(KeyCode.W))
        {
            isPressJumpButton = true;
        }
    } 

   
    private void FixedUpdate()
    {   
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
        animator.SetFloat("speed",Mathf.Abs( horizontalInput * moveSpeed));
        isGrounded = GroundCheck();
        // Kiểm tra xem nếu player đang đứng dưới ground và người chơi nhấn phím W
        if (isGrounded && isPressJumpButton)
        {
            isPressJumpButton = false;
            // Add a vertical force to the player.
            isGrounded = false;
            isWasGrounded = false;
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            //animator.SetBool("isJump", true);
            animator.SetTrigger("jump");
        } 

  
        // Kiểm tra khi nào người chơi nhấn nút "Attack" và không phải trong thời gian cooldown
        if (Input.GetMouseButton(0) && !isAttacking)
        {
            // Kích hoạt animation tấn công
            Debug.Log("Attack");
             Attack();
        }


        // điểu khiển player quay sang trái hoặc sang phải theo chiều nhấn 
        if (horizontalInput > 0 && !m_FacingRight) Flip(); 
        else if (horizontalInput < 0 && m_FacingRight) Flip();

        if (isGrounded && !isWasGrounded)
        { 
            isWasGrounded = true;
        }
    }
    bool GroundCheck()
    {
        Collider2D hit = Physics2D.OverlapCircle(groundCheck.transform.position, radius, layerMaskGround);
        if (hit != null)
        {
            Debug.Log("on ground");
            return true;
        }
        return false;
    }
    void Attack()
    {
        // Đặt biến isAttacking thành true để ngăn việc tấn công liên tục
        isAttacking = true;
          
        // Đặt tham số trong Animator Controller để kích hoạt animation tấn công
        animator.SetTrigger("attack");

        // Sau khi hoàn thành animation tấn công, đặt biến isAttacking lại thành false
        // (Nên chờ đến khi animation hoàn thành trước)
        Invoke("ResetAttack", 0.5f);
    }

    void ResetAttack()
    {
        isAttacking = false;
    } 

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "deadzone")
        {
            animator.SetTrigger("die");
        }
    }
}
