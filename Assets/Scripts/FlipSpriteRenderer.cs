using UnityEngine;

public class FlipSpriteRenderer : MonoBehaviour
{
    private SpriteRenderer _sr = null;
    private float _lastPositionX = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _sr = GetComponent<SpriteRenderer>();

        if (!_sr)
        {
            Debug.LogError("Failed to get SpriteRenderer");
        }

        // Initialize _lastPositionX with the current position
        _lastPositionX = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (_sr != null)
        {
            float currentPositionX = transform.position.x;

            // Flip sprite based on movement direction
            if (currentPositionX > _lastPositionX)
            {
                _sr.flipX = false; // Moving right
            }
            else if (currentPositionX < _lastPositionX)
            {
                _sr.flipX = true; // Moving left
            }

            // Update the last position
            _lastPositionX = currentPositionX;
        }
    }
}
