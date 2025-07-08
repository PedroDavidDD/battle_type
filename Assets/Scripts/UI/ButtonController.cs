using UnityEngine;
using UnityEngine.UI;

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
            Time.timeScale = 1f;
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
}
