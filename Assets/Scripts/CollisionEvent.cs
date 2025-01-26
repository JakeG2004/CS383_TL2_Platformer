using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
public class CollisionEvent : MonoBehaviour
{
    [SerializeField] private UnityEvent _collisionEvent;

    [SerializeField] private string _collideTag = "Player";

    // Test comment

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.CompareTag(_collideTag))
        {
            _collisionEvent.Invoke();
            gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag(_collideTag))
        {
            _collisionEvent.Invoke();
            CoinManager.Instance.AddCoin();
            gameObject.SetActive(false);
        }
    }
}
