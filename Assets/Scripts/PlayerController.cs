using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    public float radius;
    Rigidbody2D rb;
    private bool m_FacingRight = true;
    float horizontalInput;
    float verticalInput; 
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask layerMaskGround; 
    private bool isGrounded =false; // kiem tra xem player co dang dung tren ground hay khong

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.transform.position, radius);
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); 
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
    }
    void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical"); 
    }
    bool GroundCheck()
    {
        Collider2D hit = Physics2D.OverlapCircle(groundCheck.transform.position, radius, layerMaskGround);
        if(hit != null) { 
            Debug.Log("chạm");
            return true;
        }
        return false;
    }
    private void FixedUpdate()
    {   
        isGrounded = GroundCheck();
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);

        // If the player should jump...
        if (isGrounded && Input.GetKey(KeyCode.W))
        {
            Debug.Log("nhay");
            // Add a vertical force to the player.
            isGrounded = false;
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        } 

        // If the input is moving the player right and the player is facing left...
        if (horizontalInput > 0 && !m_FacingRight)
        {
            // ... flip the player.
            Flip();
        }
        // Otherwise if the input is moving the player left and the player is facing right...
        else if (horizontalInput < 0 && m_FacingRight)
        {
            // ... flip the player.
            Flip();
        }
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
}
