using UnityEngine;
using UnityEngine.UI;

public class BCModeToggle : MonoBehaviour
{
    [SerializeField] private Toggle toggle;

    void Start()
    {
        if (toggle == null)
        {
            toggle = GetComponent<Toggle>();
            if (toggle == null)
            {
                Debug.LogError("Toggle component not found on " + gameObject.name);
                return;
            }
        }

        //load the saved state
        bool savedState = PlayerPrefs.GetInt("BCMode", 0) == 1;
        
        //set the toggle without triggering the event
        toggle.isOn = savedState;
        
        //add the listener
        toggle.onValueChanged.AddListener(OnToggleValueChanged);

        Debug.Log("Initial BC Mode state: " + savedState);
    }

    public void OnToggleValueChanged(bool isOn)
    {
        PlayerPrefs.SetInt("BCMode", isOn ? 1 : 0);
        PlayerPrefs.Save();
        Debug.Log("BC Mode changed to: " + isOn);
    }
}
