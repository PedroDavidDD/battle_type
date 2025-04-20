using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text scoreText;
    public Text livesText;
    public Text activeWordsText;

    private void Update()
    {/*
        // Actualizar la puntuación y las vidas
        scoreText.text = "Score: " + GameManager.Instance.score;
        livesText.text = "Lives: " + GameManager.Instance.lives;

        // Mostrar palabras activas
        string activeWords = "";
        foreach (var enemy in FindObjectsOfType<Enemy>())
        {
            activeWords += enemy.word + "\n";
        }
        activeWordsText.text = "Active Words:\n" + activeWords;*/
    }
}