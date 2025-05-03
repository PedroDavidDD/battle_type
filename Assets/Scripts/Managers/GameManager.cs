using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int score = 0;
    public int lives = 100;

    private bool isGameOver = false; // Indica si el juego ha terminado

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        lives = 110;
    }

    public void AddScore(int points)
    {
        if (isGameOver) return; // Evitar cambios si el juego ha terminado

        score += points;
        Debug.Log("Score: " + score);
    }

    public void LoseLife()
    {
        if (isGameOver) return; // Evitar cambios si el juego ha terminado

        lives--;
        Debug.Log("Lives: " + lives);

        if (lives <= 0)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        if (isGameOver) return; // Evitar ejecutar Game Over múltiples veces

        isGameOver = true; // Marcar el juego como terminado
        Debug.Log("Game Over!");

        // Detener el tiempo del juego
        Time.timeScale = 0;

        // Aquí puedes cargar una escena de Game Over o mostrar un panel de Game Over
        // Ejemplo: SceneManager.LoadScene("GameOverScene");
    }

    public void RestartGame()
    {
        // Restablecer el tiempo del juego
        Time.timeScale = 1;

        // Reiniciar variables
        score = 0;
        lives = 10;
        isGameOver = false;

        Debug.Log("Juego reiniciado!");
        // Opcionalmente, recargar la escena actual o realizar otras acciones
        // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}