using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int score = 0;
    public int lives = 100;

    private bool isGameOver = false; // Indica si el juego ha terminado

    // Estadisticas del juego (tiempo, palabras, enemigos)
    // Tiempo de inicio del juego
    public float gameStartTime;
    // Caracteres escritos
    public int wordsTyped;
    // Enemigos eliminados
    public int enemiesKilled;
    public GameOverPanel gameOverPanel;

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

        gameStartTime = Time.time;
    }

    private void Start()
    {
        lives = 5;
        gameOverPanel = GameObject.Find("GameOverPanel").GetComponent<GameOverPanel>();
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
            // Mostrar el panel del Game Over
            ShowGameOver();
        }
    }

    public void ShowGameOver()
    {
        Debug.Log("gameOverPanel: " + gameOverPanel);
        if (gameOverPanel != null)
        {
            gameOverPanel.ShowPanel();
            Debug.Log(" Mostrar panel del Game Over!");
        }
        GameOver();
    }

    private void GameOver()
    {
        if (isGameOver) return; // Evitar ejecutar Game Over multiples veces

        isGameOver = true; // Marcar el juego como terminado
        Debug.Log("Game Over!");

        // Detener el tiempo del juego
        Time.timeScale = 0;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

    public void HideGameOver()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.gameObject.SetActive(false);
        }
        Time.timeScale = 1;
    }

    public void RestartGame()
    {
        // Restablecer el tiempo del juego
        Time.timeScale = 1;

        // Reiniciar variables
        score = 0;
        lives = 10;
        isGameOver = false;
        
        // Restablecer el tiempo del juego
        gameStartTime = Time.time;      
        // Caracteres escritos
        wordsTyped = 0;
        // Enemigos eliminados
        enemiesKilled = 0;  

        Debug.Log("Juego reiniciado!");
        // Opcionalmente, recargar la escena actual o realizar otras acciones
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void AddWordTyped()
    {
        this.wordsTyped++;
    }

    public void AddEnemyKilled()
    {
        this.enemiesKilled++;
    }
}