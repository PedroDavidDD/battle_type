using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text scoreText;
    public Text livesText;
    public Text activeWordsText;
    public Text inputText;

    private void Start()
    {
    }

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
        string currentInputText = GameObject.Find("InputHandler").GetComponent<InputHandler>().GetCurrentInput();

        inputText.text = currentInputText;
    }
}