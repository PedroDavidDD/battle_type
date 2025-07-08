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
        inputText.text = currentInputText;

        // Verificar coincidencias de caracteres con las palabras de los enemigos
        CheckCharacterMatch(currentInputText);

        GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        livesText.text = "" + gameManager.lives.ToString();
        scoreText.text = "Puntaje: " + gameManager.score.ToString();
        levelText.text = "Nivel: " + gameManager.currentLevel.ToString();
        
        // Obtener el tiempo actual del juego
        float gameTime = gameManager.GetGameTime();
        
        // Formatear el tiempo como minutos:segundos
        int minutes = Mathf.FloorToInt(gameTime / 60);
        int seconds = Mathf.FloorToInt(gameTime % 60);
        
        timeText.text = $"Tiempo: {minutes:D2}:{seconds:D2}";
    }

    private void CheckCharacterMatch(string currentInputText)
    {
        // Iterar sobre todos los enemigos activos
        foreach (var enemyObject in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Enemy enemy = enemyObject.GetComponent<Enemy>();
            if (enemy == null) continue; // Saltar si falta el script Enemy

            string word = enemy.GetEnemyWord(); // Palabra asignada al enemigo

            // Buscar el hijo "Body" del enemigo
            Transform bodyTransform = enemyObject.transform.Find("Body");
            if (bodyTransform == null)
            {
                Debug.LogWarning($"El enemigo {enemyObject.name} no tiene un hijo llamado 'Body'.");
                continue;
            }

            // Acceder al SpriteRenderer del hijo "Body"
            SpriteRenderer bodySpriteRenderer = bodyTransform.GetComponent<SpriteRenderer>();
            if (bodySpriteRenderer == null)
            {
                Debug.LogWarning($"El hijo 'Body' del enemigo {enemyObject.name} no tiene un SpriteRenderer.");
                continue;
            }

            // Manejar el caso cuando la entrada del jugador esta vacia
            if (string.IsNullOrEmpty(currentInputText))
            {
                // Restaurar el color del SpriteRenderer del hijo "Body" a blanco
                bodySpriteRenderer.color = Color.white;
                continue;
            }

            // Comparar la entrada del jugador con la palabra del enemigo
            if (currentInputText.Length <= word.Length && word.StartsWith(currentInputText))
            {
                activeWordsText.text = "Word: " + word;

                // Cambiar el color del SpriteRenderer del hijo "Body" a amarillo
                bodySpriteRenderer.color = new Color(1f, 0.729f, 0.082f); // Amarillo aproximado
            }
            else
            {
                // Restaurar el color del SpriteRenderer del hijo "Body" a blanco
                bodySpriteRenderer.color = Color.white;
            }
        }
    }

}