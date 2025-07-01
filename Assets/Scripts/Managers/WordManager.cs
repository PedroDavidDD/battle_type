using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;

public class WordManager : MonoBehaviour
{
    private List<Enemy> activeEnemies = new List<Enemy>();

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform shootPoint;
    private GameManager gameManager;
    
    // Rastrea el progreso por cada palabra que se registra
    private Dictionary<string, int> processedCharacters = new Dictionary<string, int>();
    // Palabra seleccionada
    private string selectedWord = "";
    
    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        InitializeEnemies();
    }

    // Método para inicializar los enemigos
    public void InitializeEnemies()
    {
        // Limpiar la lista de enemigos
        activeEnemies.Clear();
        processedCharacters.Clear();

        // Buscar y registrar todos los enemigos en la escena
        Enemy[] enemies = FindObjectsOfType<Enemy>();
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
        activeEnemies.Remove(enemy);
    }
    public void CheckInput(string currentInput)
    {
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
}