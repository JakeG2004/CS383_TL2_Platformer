using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // Keys
    [SerializeField] private KeyCode _jump = KeyCode.W;

    // Variable to get x axis for movement From input manager
    private float _move;

    // useful values to change
    [SerializeField] private float _maxSpeed = 10.0f;
    [SerializeField] private float _jumpForce = 8.0f;
    [SerializeField] private float _friction = 10.0f;
    [SerializeField] private float _fallThreshold = -10.0f;


    // Private variables
    private Rigidbody2D _rb = null;
    private bool _isGrounded = false;

    // Animator
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    
    bool facingRight = true;
    //Starting position
    private Vector2 _startingPosition;

    //Health
    public Image healthBar;
    public CanvasGroup gameOverCanvas;
    private float healthAmount = 100f;
    private float damage = 50f;

    //BC Mode
    private bool bcMode = false;




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
        
        

        //Save starting position
        _startingPosition = transform.position;

        //Initialize health bar
        healthBar.fillAmount = healthAmount / 100f;

        //Initially make game over canvas invisible
        gameOverCanvas.alpha = 0f;
        gameOverCanvas.interactable = false;
        gameOverCanvas.blocksRaycasts = false;

        //Load BC mode setting check (jillian)
        UpdateBCMode();
    }

    // Update is called once per frame
    void Update()
    {
        GetPlayerMovement();
        CheckGround();

        UpdateBCMode(); //checks if exist (jillian)

        if(Input.GetKeyDown("r") || Input.GetButtonDown("Submit"))
        {
            ExitGameOver();
        }


        if(transform.position.y < _fallThreshold || healthAmount <= 0)
        {
            ResetToStart();
            TriggerGameOver();

            /*
            if(bcMode)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            else
            {
                ResetToStart();
                TriggerGameOver();
            }
            */
        }
        if (Input.GetKey(KeyCode.Q))
        {
            Application.Quit();
        }
    }

    void GetPlayerMovement()
    {
        // Handle horizontal movement
        _move = Input.GetAxis("Horizontal");
        if(_move < 0)
        {
            _rb.linearVelocityX = -1 * _maxSpeed;
            
            if(facingRight){
                Flip();
            }
            animator.SetBool("IsRunning", true);
        }

        else if(_move > 0)
        {
            _rb.linearVelocityX = _maxSpeed;
            if(!facingRight){
                Flip();
            }
            animator.SetBool("IsRunning", true);
        

           _rb.linearVelocity = new Vector2(_move*_maxSpeed,_rb.linearVelocity.y);
        }
        // Lerp to zero velocity
        else
        {
            _rb.linearVelocityX = Mathf.Lerp(_rb.linearVelocityX, 0.0f, Time.deltaTime * _friction);
            animator.SetBool("IsRunning", false);
        }

        // Handle vertical movement
        if((Input.GetKeyDown(_jump) || Input.GetButtonDown("Jump")) && _isGrounded)
        {
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, _jumpForce);
            
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
        if(!bcMode){
        //Change player's color to red upon impact
        GetComponent<SpriteRenderer>().color = Color.red;
        animator.SetTrigger("TriggerHurt");
        TakeDamage(damage);
        }
    }

    public void TakeDamage(float damage)
    {
        if(!bcMode){ //(jillian)
            healthAmount -= damage;
            healthAmount = Mathf.Clamp(healthAmount, 0, 100);
            healthBar.fillAmount = healthAmount / 100f;
            _maxSpeed = 7f;
        }
    }

    private void UpdateBCMode(){
        bcMode = PlayerPrefs.GetInt("BCMode", 0) == 1;
    }
    void TriggerGameOver()
    {
        //gameOverCanvas.alpha = 1f;
        //gameOverCanvas.interactable = true;
        //gameOverCanvas.blocksRaycasts = true;

        SceneManager.LoadScene(2); //(andrew)

    }

    void ExitGameOver()
    {
        gameOverCanvas.alpha = 0f;
        gameOverCanvas.interactable = false;
        gameOverCanvas.blocksRaycasts = false;
    }

    void ResetToStart()
    {
        transform.position = _startingPosition;

        //Reset velocity to prevent momentum
        _rb.linearVelocity = Vector2.zero;
    }

}
