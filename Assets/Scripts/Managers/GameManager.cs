using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

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
    [Header("Enemy deleted")]
    public int enemiesKilled;
    
    public int currentLevel = 0;
    public int enemiesPerLevel = 5;

    [Header("Enemy data")]
    public int totalEnemiesToSpawn = 0;  // Total de enemigos que deben aparecer

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        // Inicializar el tiempo al inicio
        gameStartTime = Time.time;
    }

    private void Start()
    {
        lives = 5;
    }
    
    public void AddScore(int points)
    {
        if (isGameOver) return;

        score += points;
    }

    public void AddLevel()
    {
        currentLevel++;
        Debug.Log("Level: " + currentLevel);
    }   

    public void LoseLife()
    {
        if (isGameOver) return;

        lives--;

        if (lives <= 0)
        {
            // Mostrar el panel del Game Over
            UIManager.Instance.ShowGameOver();
            
            Player.Instance.playerSoundController.PlayMuerteJugadorSound();
        } else {
            UIManager.Instance.HideGameOver();
        }
    }

    public void GameOver()
    {
        if (isGameOver) return; // Evitar ejecutar Game Over multiples veces

        isGameOver = true; // Marcar el juego como terminado

        // Detener el tiempo del juego
        Time.timeScale = 0;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

    public void RestartGame()
    {
        // Primero restablecer el tiempo
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

        // Reiniciar variables
        score = 0;
        lives = 5;
        isGameOver = false;
        currentLevel = 0;
        wordsCompleted = 0;
        enemiesKilled = 0;
        
        Debug.Log("Juego reiniciado!");

        // Recargar la escena actual
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        
        // Reiniciar el tiempo justo después de cargar la escena
        gameStartTime = Time.time;
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

    public float GetGameTime()
    {
        return Time.time - gameStartTime;
    }

}