using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverPanel;

    private void Start()
    {
        //gameOverPanel.SetActive(false);
    }

    public void ShowGameOver()
    {
        //gameOverPanel.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Debug.Log("RestartGame!");
    }
}