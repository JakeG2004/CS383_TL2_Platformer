using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Keys
    [SerializeField] private KeyCode _left = KeyCode.A;
    [SerializeField] private KeyCode _right = KeyCode.D;
    [SerializeField] private KeyCode _jump = KeyCode.W;

    // useful values to change
    [SerializeField] private float _maxSpeed = 10.0f;
    [SerializeField] private float _jumpForce = 8.0f;
    [SerializeField] private float _friction = 10.0f;

    // Private variables
    private Rigidbody2D _rb = null;
    private bool _isGrounded = false;

    // Animator
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    
    bool facingRight = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get rigidbody
        _rb = GetComponent<Rigidbody2D>();

        if(!_rb)
        {
            Debug.Log("Failed to get rigidbody!");
        }

        // Assign Animator component
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        GetPlayerMovement();
        CheckGround();
    }

    void GetPlayerMovement()
    {
        // Handle horizontal movement
        if(Input.GetKey(_left))
        {
            _rb.linearVelocityX = -1 * _maxSpeed;
            
            if(facingRight){
                Flip();
            }
            animator.SetBool("IsRunning", true);
            //animator.Play("Character_Run");
        }

        else if(Input.GetKey(_right))
        {
            _rb.linearVelocityX = _maxSpeed;
            if(!facingRight){
                Flip();
            }
            animator.SetBool("IsRunning", true);
            //animator.Play("Character_Run");
        }

        // Lerp to zero velocity
        else
        {
            _rb.linearVelocityX = Mathf.Lerp(_rb.linearVelocityX, 0.0f, Time.deltaTime * _friction);
            animator.SetBool("IsRunning", false);
            //animator.Play("Character_Idle");
        }

        // Handle vertical movement
        if(Input.GetKeyDown(_jump) && _isGrounded)
        {
            _rb.linearVelocityY = _jumpForce;
            //animator.Play("Character_Jump");
        }
    }

    void CheckGround()
    {
        // Raycast to ground
        RaycastHit2D groundHit = Physics2D.Raycast(transform.position, -1 * Vector2.up);

        // On hit
        if(groundHit)
        {
            // Find distance
            float groundDist = Mathf.Abs(groundHit.point.y - transform.position.y);

            // Update isGrounded based on player y scale
            if(groundDist < (transform.localScale.y / 2) + 0.1f)
            {
                _isGrounded = true;

                // End animation for jumping
                animator.SetBool("IsJumping", false);
            }

            else
            {
                _isGrounded = false;
                animator.SetBool("IsJumping", true);
            }
        }

        else
        {
            _isGrounded = false;
        }
    }

    void Flip(){
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;

        facingRight = !facingRight;
    }

    public void Hurt()
    {
        // Populate
        Debug.Log("Player hurt");
        animator.SetTrigger("TriggerHurt");
    }
}
