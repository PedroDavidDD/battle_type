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

    private void Update()
    {
        // Obtener la entrada actual del jugador
        string currentInputText = GameObject.Find("InputHandler").GetComponent<InputHandler>().GetCurrentInput();
       
        // Verificar coincidencias de caracteres con las palabras de los enemigos
        GameObject.Find("WordManager").GetComponent<WordManager>().CheckCharacterMatch(currentInputText, activeWordsText);

        GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        livesText.text = "" + gameManager.lives.ToString();
        scoreText.text = "Puntaje: " + gameManager.score.ToString();
        levelText.text = "Nivel: " + gameManager.currentLevel.ToString();
        inputText.text = currentInputText;

        // Obtener el tiempo actual del juego
        float gameTime = gameManager.GetGameTime();
        
        // Formatear el tiempo como minutos:segundos
        int minutes = Mathf.FloorToInt(gameTime / 60);
        int seconds = Mathf.FloorToInt(gameTime % 60);
        
        timeText.text = $"Tiempo: {minutes:D2}:{seconds:D2}";
    }
}