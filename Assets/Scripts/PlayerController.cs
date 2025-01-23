using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private KeyCode _left = KeyCode.A;
    [SerializeField] private KeyCode _right = KeyCode.D;

    private Rigidbody2D rb = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if(!rb)
        {
            Debug.Log("Failed to get rigidbody!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        GetPlayerMovement();
    }

    void GetPlayerMovement()
    {
        if(Input.GetKey(_left))
        {
            rb.linearVelocityX = 10.0f;
        }
    }
}
