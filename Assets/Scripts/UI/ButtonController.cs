using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
    public bool isPaused = false;
    public GameObject pausePanel;
    public GameObject txtInput;

    private void Start()
    {
        pausePanel.SetActive(false);
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        
        if (isPaused)
        {
            // Pausar el juego
            Time.timeScale = 0f;
            if (pausePanel != null)
            {
                pausePanel.SetActive(true);
            }
            if (txtInput != null)
            {
                txtInput.SetActive(false);
            }
        }
        else
        {
            // Reanudar el juego
            ResetTime();
            
            if (pausePanel != null)
            {
                pausePanel.SetActive(false);
            }
            if (txtInput != null)
            {
                txtInput.SetActive(true);
            }
        }
    }
    
    public void RestartGame()
    {
        TogglePause();
        SceneManager.LoadScene( SceneManager.GetActiveScene().buildIndex );
    }
    
    public void Menu()
    {
        TogglePause();
        SceneManager.LoadScene(0);
    }
    
    public void Quit()
    {
        TogglePause();
        Application.Quit();
    }
    
    public void GoToCategories()
    {
        TogglePause();
        SceneManager.LoadScene(1);
    }
    // Resetear el tiempo normal 
    public void ResetTime()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
    }
    
}
