using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordManager : MonoBehaviour
{
    private List<Enemy> activeEnemies = new List<Enemy>();

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform shootPoint;
    private GameManager gameManager;
    [SerializeField] private PlayerSoundController playerSoundController;
    
    // Rastrea el progreso por cada palabra que se registra
    private Dictionary<string, int> processedCharacters = new Dictionary<string, int>();
    // Palabra seleccionada
    private string selectedWord = "";
    public ButtonController buttonController;
    public EnemySpawner enemySpawner;

    private void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        enemySpawner = FindFirstObjectByType<EnemySpawner>();

        InitializeEnemies();   
    }

    // Método para inicializar los enemigos
    public void InitializeEnemies()
    {
        // Limpiar la lista de enemigos
        activeEnemies.Clear();
        processedCharacters.Clear();

        // Buscar y registrar todos los enemigos en la escena
        Enemy[] enemies = GameObject.FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        foreach (Enemy enemy in enemies)
        {
            RegisterEnemy(enemy);
        }
    }
    
    public void RegisterEnemy(Enemy enemy)
    {
        activeEnemies.Add(enemy);
    }

    public void UnregisterEnemy(Enemy enemy)
    {
        if (activeEnemies.Remove(enemy))
        {
            Debug.Log($"Enemigo eliminado. Enemigos restantes: {activeEnemies.Count}");
        }
    }
    
    public void CheckInput(string currentInput)
    {
        if (buttonController){
            if (buttonController.isPaused) return;
        }

        foreach (var enemy in activeEnemies)
        {
            selectedWord = enemy.GetEnemyWord();

            if (currentInput.Length <= selectedWord.Length && selectedWord.StartsWith(currentInput))
            {
                // Modifica el diccionario para asegurarse de que una clave exista - inicializa las palabras
                processedCharacters[selectedWord] = processedCharacters.ContainsKey(selectedWord)
                    ? processedCharacters[selectedWord] : 0;

                ForCharacters(currentInput, enemy);
                // ForWord(input, enemyWord);
            }
        }
        LogDictionaryContents();
    }
    /// <summary>
    /// Compara los caracteres ingresados por el jugador con la palabra del enemigo actual.
    /// Dispara balas por cada caracter coincidente y actualiza el progreso de la palabra.
    /// </summary>
    /// <param name="input">La entrada del jugador (lo que está escribiendo)</param>
    /// <param name="enemy">El enemigo al que se está comparando la palabra</param>
    private void ForCharacters(string input, Enemy enemy)
    {
        // Contador de caracteres que coinciden
        int matchCount = 0;

        // Obtener el progreso actual de la palabra
        // Si no existe en el diccionario, se inicializa en 0
        int processedCount = processedCharacters.ContainsKey(selectedWord) 
            ? processedCharacters[selectedWord] : 0;

        // Recorre los caracteres desde donde se quedó la última vez
        for (int i = processedCount; i < input.Length && i < selectedWord.Length; i++)
        {
            // Si el caracter coincide con la palabra del enemigo
            if (input[i] == selectedWord[i])
            {
                matchCount++;
            }
            else
            {
                break;
            }
        }

        // Si hay caracteres que coinciden
        if (matchCount > 0)
        {
            // Disparar una bala por cada caracter correcto
            ShootBullet(enemy.transform);
            // Actualizar el progreso de la palabra en el diccionario
            processedCharacters[selectedWord] += matchCount;

            // Verificar si la palabra está completa
            if (processedCharacters[selectedWord] >= selectedWord.Length)
            {
                // Solo incrementar el contador cuando la palabra está completa
                gameManager.AddWordCompleted();
            }
        }

        Debug.Log($"Caracteres coincidentes: {matchCount}");
        Debug.Log($"Palabra objetivo: {selectedWord}");
    }
    // Dispara una bala en una posicion especifica.
    private void ShootBullet(Transform target)
    {
        if (bulletPrefab == null || shootPoint == null)
        {
            Debug.LogError("Falta asignar el prefab de la bala o el punto de disparo.");
            return;
        }

        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);

        // Asignar el objetivo a la bala
        Ball ballScript = bullet.GetComponent<Ball>();
        Debug.Log("ballScript "+ballScript);
        if (ballScript != null)
        {
            ballScript.SetTarget(target);
            Debug.Log("ballScript actualizado el Target" + ballScript);
        }
        // Reproduce el sonido de disparo
        playerSoundController.PlayDisparoSound();
    }
    // Compara la entrada con la palabra seleccionada
    private void ForWord(string input, string enemyWord)
    {
        if (input == enemyWord)
        {
            Debug.Log("Palabra correcta. Destruyendo enemigo: " + enemyWord);
            Destroy(gameObject); // Destruir el enemigo

            EnemyPoints();
        }
        else
        {
            Debug.Log(input + " Palabra INCORRECTA: " + enemyWord);
        }
    }

    private void EnemyPoints()
    {
        int points = 10;
        gameManager.AddScore(points);
    }

    private void LogDictionaryContents()
    {
        Debug.Log("Estado actual del diccionario 'processedCharacters':");
        foreach (var entry in processedCharacters)
        {
            Debug.Log($"Palabra: {entry.Key}, Progreso: {entry.Value}");
        }
    }
    
    // Comprobar si la entrada del jugador coincide con las palabras de los enemigos para actualizar el color del enemigo y las palabras activas en el UI
    public void CheckCharacterMatch(string currentInputText, Text activeWordsText)
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
                activeWordsText.text = "Enemigo: " + word;

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