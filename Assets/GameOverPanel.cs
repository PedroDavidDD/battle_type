using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{
    public Text timePlayedText;
    public Text wordsPerSecondText;
    public Text wordsCompletedText;
    public Text enemiesKilledText;
    public Text finalScoreText;

    private GameManager gameManager;
    private float finalTime;
    private int finalWordsCompleted;
    private int finalEnemiesKilled;
    private int finalScore;

    private void Start()
    {
        gameManager = GameManager.Instance;
        gameObject.SetActive(false); // Iniciar oculto
    }

    public void ShowPanel()
    {
        gameObject.SetActive(true);
        // Guardar los valores finales
        finalTime = Time.time - gameManager.gameStartTime;
        finalWordsCompleted = gameManager.wordsCompleted;
        finalEnemiesKilled = gameManager.enemiesKilled;
        finalScore = gameManager.score;
        UpdateStatistics();
    }

    private void UpdateStatistics()
    {
        float wordsPerSecond = finalWordsCompleted / finalTime;

        timePlayedText.text = $"Tiempo jugado: {finalTime:F2} segundos";
        // Si llega al minuto
        if (finalTime >= 60)
        {
            timePlayedText.text = $"Tiempo jugado: {finalTime / 60:F2} minutos";
        }   
        // Si llega a la hora
        if (finalTime >= 3600)
        {
            timePlayedText.text = $"Tiempo jugado: {finalTime / 3600:F2} horas";
        }
        wordsCompletedText.text = $"Palabras completadas: {finalWordsCompleted}";
        wordsPerSecondText.text = $"Palabras completadas por segundo: {wordsPerSecond:F2}";
        enemiesKilledText.text = $"Enemigos eliminados: {finalEnemiesKilled}";
        finalScoreText.text = $"Puntuación final: {finalScore}";
    }
}
