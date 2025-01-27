using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ControllerManager : MonoBehaviour
{
    public GameObject PlayButton;
    public GameObject QuitButton;
    private float _select;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _select = Input.GetAxis("Vertical");
        if(_select == 1)
        {
            EventSystem.current.SetSelectedGameObject(PlayButton);
        }
        if(_select == -1)
        {
            EventSystem.current.SetSelectedGameObject(PlayButton);
        }
        
    }
}
