using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

//THIS SCRIPT IS FOR THE PAUSE MENUUUUUU YUHHYHHHHHH
public class PauseManager : MonoBehaviour
{
    public GameObject PauseMenu;
    public static bool isPaused; //make global variable so no other inputs during pause
    void Start(){
        PauseMenu.SetActive(false);
    }


    void Update(){
        if(Input.GetKeyDown(KeyCode.Escape) || (Gamepad.current != null && Gamepad.current.startButton.wasPressedThisFrame)){
            if(isPaused){
                resumeGame();
            }else{
                pauseGame();
            }
        }
    }

    public void pauseGame(){
        PauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

    }

    public void resumeGame(){
        PauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void mainMenu(){
        Time.timeScale = 1f;
        SceneManager.LoadScene("mainMenu");
    }

    public void quitGame(){
        Application.Quit();//comment
    }
}

