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
    public int wordsCompleted;
    // Enemigos eliminados
    public int enemiesKilled;
    public GameOverPanel gameOverPanel;
    
    public int currentLevel = 0;
    public int enemiesPerLevel = 5;

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
        gameOverPanel = gameOverPanel.GetComponent<GameOverPanel>();
    }

    public void AddScore(int points)
    {
        if (isGameOver) return; // Evitar cambios si el juego ha terminado

        score += points;
        Debug.Log("Score: " + score);
    }

    public void AddLevel()
    {
        currentLevel++;
        Debug.Log("Level: " + currentLevel);
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
        if (gameOverPanel != null)
        {
            gameOverPanel.ShowPanel();
        }
        GameOver();
    }

    private void GameOver()
    {
        if (isGameOver) return; // Evitar ejecutar Game Over multiples veces

        isGameOver = true; // Marcar el juego como terminado

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
        currentLevel = 0;
        
        // Restablecer el tiempo del juego
        gameStartTime = 0f;      
        // Palabras completadas
        wordsCompleted = 0;
        // Enemigos eliminados
        enemiesKilled = 0;  

        Debug.Log("Juego reiniciado!");
        // Recargar la escena actual
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        
        // Obtener referencia al WordManager y inicializar los enemigos
        WordManager wordManager = FindObjectOfType<WordManager>();
        if (wordManager != null)
        {
            wordManager.InitializeEnemies();
        }
    }

    public void AddWordCompleted()
    {
        this.wordsCompleted++;
    }

    public void AddEnemyKilled()
    {
        this.enemiesKilled++;

        if (this.enemiesKilled % enemiesPerLevel == 0)
        {
            AddLevel();
        }
    }
}