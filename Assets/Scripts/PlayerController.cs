using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    [SerializeField] private float _fallThreshold = -10.0f;


    // Private variables
    private Rigidbody2D _rb = null;
    private bool _isGrounded = false;

    //Starting position
    private Vector2 _startingPosition;

    //Health
    public Image healthBar;
    public CanvasGroup gameOverCanvas;
    private float healthAmount = 100f;
    private float damage = 50f;

    //BC Mode
    public bool BC = false;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get rigidbody
        _rb = GetComponent<Rigidbody2D>();

        if(!_rb)
        {
            Debug.Log("Failed to get rigidbody!");
        }

        //Save starting position
        _startingPosition = transform.position;

        //Initialize health bar
        healthBar.fillAmount = healthAmount / 100f;

        //Initially make game over canvas invisible
        gameOverCanvas.alpha = 0f;
        gameOverCanvas.interactable = false;
        gameOverCanvas.blocksRaycasts = false;
    }

    // Update is called once per frame
    void Update()
    {
        GetPlayerMovement();
        CheckGround();

        if(transform.position.y < _fallThreshold || healthAmount <= 0)
        {
            if (BC)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            else
            {
                ResetToStart();
                TriggerGameOver();
            }
        }

    }

    void GetPlayerMovement()
    {
        // Handle horizontal movement
        if(Input.GetKey(_left))
        {
            _rb.linearVelocityX = -1 * _maxSpeed;
        }

        else if(Input.GetKey(_right))
        {
            _rb.linearVelocityX = _maxSpeed;
        }

        // Lerp to zero velocity
        else
        {
            _rb.linearVelocityX = Mathf.Lerp(_rb.linearVelocityX, 0.0f, Time.deltaTime * _friction);
        }

        // Handle vertical movement
        if(Input.GetKeyDown(_jump) && _isGrounded)
        {
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
            if(groundDist < (transform.localScale.y / 2) + 0.1f)
            {
                _isGrounded = true;
            }

            else
            {
                _isGrounded = false;
            }
        }

        else
        {
            _isGrounded = false;
        }
    }
    public void Hurt()
    {
        //Change player's color to red upon impact
        GetComponent<SpriteRenderer>().color = Color.red;

        TakeDamage(damage);
    }

    public void TakeDamage(float damage)
    {
        healthAmount -= damage;
        healthAmount = Mathf.Clamp(healthAmount, 0, 100);
        healthBar.fillAmount = healthAmount / 100f;
        _maxSpeed = 7f;
    }

    void TriggerGameOver()
    {
        gameOverCanvas.alpha = 1f;
        gameOverCanvas.interactable = true;
        gameOverCanvas.blocksRaycasts = true;
    }

    void ResetToStart()
    {
        transform.position = _startingPosition;

        //Reset velocity to prevent momentum
        _rb.linearVelocity = Vector2.zero;
    }

}
