using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsManagement : MonoBehaviour
{
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Called when Quit button is pressed
    public void QuitGame()
    {
        Application.Quit(); 
    }
}
