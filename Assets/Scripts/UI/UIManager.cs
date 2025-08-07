using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text scoreText;
    public Text livesText;
    public Text levelText;
    public Text activeWordsText;
    public Text inputText;
    public Text timeText;

    [Header("Variable para el panel de Game Over")]
    public GameOverPanel gameOverPanel;
    [Header("Variable para el texto de la entrada")]
    public GameObject txtInput;
    public GameManager gameManager;

    public static UIManager Instance { get; private set; }

    private void Awake() {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void Start() {
        
        gameOverPanel = gameOverPanel.GetComponent<GameOverPanel>();
        txtInput = GameObject.Find("Panel");
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void Update()
    {
        // Obtener la entrada actual del jugador
        string currentInputText = GameObject.Find("InputHandler").GetComponent<InputHandler>().GetCurrentInput();
       
        // Verificar coincidencias de caracteres con las palabras de los enemigos
        GameObject.Find("WordManager").GetComponent<WordManager>().CheckCharacterMatch(currentInputText, activeWordsText);

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        livesText.text = gameManager.lives.ToString();
        scoreText.text = gameManager.score.ToString();
        levelText.text = gameManager.currentLevel.ToString();
        inputText.text = currentInputText;

        // Obtener el tiempo actual del juego
        float gameTime = gameManager.GetGameTime();
        
        // Formatear el tiempo como minutos:segundos
        int minutes = Mathf.FloorToInt(gameTime / 60);
        int seconds = Mathf.FloorToInt(gameTime % 60);
        
        timeText.text = $"{minutes:D2}:{seconds:D2}";
    }

    public void ShowGameOver()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.ShowPanel();
        }
        
        gameManager.GameOver();
        
        if (txtInput != null)
        {
            txtInput.SetActive(false);
        }
    }
    
    public void HideGameOver()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.gameObject.SetActive(false);
        }

        // Primero restablecer el tiempo
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

        if (txtInput != null)
        {
            txtInput.SetActive(true);
        }
    }
    
    // metodo para reiniciar el juego
    public void RestartGame()
    {
        gameManager.RestartGame();
    }
}