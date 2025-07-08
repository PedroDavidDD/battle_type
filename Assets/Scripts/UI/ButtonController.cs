using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    private bool isPaused = false;
    public GameObject pausePanel;

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
        }
        else
        {
            // Reanudar el juego
            Time.timeScale = 1f;
            if (pausePanel != null)
            {
                pausePanel.SetActive(false);
            }
        }
    }
}
