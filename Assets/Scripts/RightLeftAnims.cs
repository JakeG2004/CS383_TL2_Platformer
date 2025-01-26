using UnityEngine;

public class MovementSerializer : MonoBehaviour
{
    private Animator animator; // Reference to the Animator component

    private Vector3 lastPosition;

    void Start()
    {
        lastPosition = transform.position;

        // Optionally, get the Animator component automatically
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    void Update()
    {
        Vector3 movement = transform.position - lastPosition;

        // Update animator parameters
        if (animator != null)
        {
            animator.SetBool("MovingLeft", movement.x < 0);
            animator.SetBool("MovingRight", movement.x > 0);
        }

        lastPosition = transform.position;
    }
}