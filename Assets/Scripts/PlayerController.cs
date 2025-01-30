using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;

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
    private float healthAmount = 100f;

    //BC Mode
    private bool bcMode = false;


    private Vector2 _lastPos;
	
	private bool powerup = false;

    //Audio Variables
    private AudioSource PlayerSFX;
    public AudioClip[] JumpSound;
    public AudioClip[] HitSound;

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

        //Load BC mode setting check (jillian)
        UpdateBCMode();

        _lastPos = _startingPosition;

        PlayerSFX = GameObject.FindWithTag("PlayerSFX").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        GetPlayerMovement();
        CheckGround();

        UpdateBCMode(); //checks if exist (jillian)


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
        
        // Take input from gamepad if it's being used
        if( Gamepad.current != null )
        {
            float joystick = Gamepad.current.leftStick.x.ReadValue();
            if( joystick != 0 )
            {
                _move = joystick;
            }
        }
        // Movement
        if(_move != 0)
        {
            if ((_move > 0 && !facingRight) || (_move < 0 && facingRight))
            {
                Flip();
            }

            _rb.linearVelocityX = _maxSpeed * _move;

            animator.SetBool("IsRunning", true);

            // Player stuck
            //if(_lastPos == new Vector2(transform.position.x, transform.position.y));
            //{
                //transform.position = new Vector2(transform.position.x + (_rb.linearVelocityX * Time.deltaTime), (transform.position.y + 0.01f));
            //}

            _lastPos = transform.position;
        }
        
        // Lerp to zero velocity
        else
        {
            _rb.linearVelocityX = Mathf.Lerp(_rb.linearVelocityX, 0.0f, Time.deltaTime * _friction);
            animator.SetBool("IsRunning", false);
        }

        // Handle vertical movement
        if((Input.GetKeyDown(_jump) || (Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame)) && _isGrounded)
        {
            PlayJumpSound();
            _rb.linearVelocityY = _jumpForce;
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
            if(groundDist < (transform.localScale.y / 2) + 0.2f)
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

    void Flip()
    {
        // Toggle facingRight
        facingRight = !facingRight;

        // Flip the player's scale
        /*Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1; // Invert the x-scale
        gameObject.transform.localScale = currentScale;*/

        spriteRenderer.flipX = !spriteRenderer.flipX;
    }

    public void Hurt(int curDamage)
    {
        // Populate
        Debug.Log("Player hurt");

        animator.SetTrigger("TriggerHurt");

        if(!bcMode)
        {
            PlayHurtSound();
            //animator.SetTrigger("TriggerHurt");
            TakeDamage(curDamage);
        }
    }

    public void TakeDamage(float damage)
    {
		// If we're powered up, power down and don't take damage.
		if (powerup) {
			Powerdown();
		}
        else if(!bcMode){ //(jillian)
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

    void ResetToStart()
    {
        transform.position = _startingPosition;

        //Reset velocity to prevent momentum
        _rb.linearVelocity = Vector2.zero;
    }
    public void PlayHurtSound()
    {
        if (HitSound.Length > 0 && PlayerSFX != null)
        {
            int randomIndex = Random.Range(0, HitSound.Length);
            AudioClip selectedClip = HitSound[randomIndex];

            PlayerSFX.PlayOneShot(selectedClip);
        }
        else
        {
            Debug.LogWarning("Hit sound is not assigned");
        }
    }

    public void PlayJumpSound()
    {
        if (JumpSound.Length > 0 && PlayerSFX != null)
        {
            int randomIndex = Random.Range(0, JumpSound.Length);
            AudioClip selectedClip = JumpSound[randomIndex];

            PlayerSFX.PlayOneShot(selectedClip);
        }
        else
        {
            Debug.LogWarning("Jump sound is not assigned");
        }
    }
	
	public void Powerup()
	{
		if (!powerup)
		{
		powerup = true;
		gameObject.transform.localScale = new Vector3(2, 2, 2);
		_isGrounded = true;
		}
		else
		{
			healthAmount += 10;
			healthAmount = Mathf.Clamp(healthAmount, 0, 100);
            healthBar.fillAmount = healthAmount / 100f;
		}
	}
	
	private void Powerdown()
	{
		powerup = false;
		gameObject.transform.localScale = new Vector3(1, 1, 1);
	}
}
