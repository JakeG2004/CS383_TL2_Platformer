using UnityEngine;

public class MobileUI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log(Application.platform);
        if( Application.platform != RuntimePlatform.Android )
        {
            //gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
