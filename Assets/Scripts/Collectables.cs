using UnityEngine;
using UnityEngine.UI;

public class Collectables : MonoBehaviour
{
    
    public Text CoinCount;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (gameObject.CompareTag("Collectables"))
            {
                int currentCount = int.Parse(CoinCount.text);
                currentCount += 5;
                CoinCount.text = currentCount.ToString();
                Destroy(gameObject);
            }
        }
    }
}