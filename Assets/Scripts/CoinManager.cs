using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance { get; private set; }

    private int _coinCount;

    [SerializeField] private TextMeshProUGUI _coinCounterText; // Reference to TextMeshPro UI element

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateCoinText(); // Initialize the text
    }

    public void AddCoin()
    {
        _coinCount++;
        UpdateCoinText(); // Update the text whenever a coin is collected
    }

    private void UpdateCoinText()
    {
        if (_coinCounterText != null)
        {
            _coinCounterText.text = "Coins: " + _coinCount;
        }
    }

    public int GetCoinCount()
    {
        return _coinCount;
    }
}
