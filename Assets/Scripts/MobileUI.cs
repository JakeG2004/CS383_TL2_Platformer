using UnityEngine;

public class MobileUI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.SetActive(Application.isMobilePlatform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
