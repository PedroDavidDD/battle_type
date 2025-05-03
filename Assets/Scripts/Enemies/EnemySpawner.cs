using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Prefab del enemigo
    public float spawnInterval = 4f; // Intervalo entre spawns
    public string[] wordList = { "apple", "banana", "car", "dog", "elephant" }; // Lista de palabras
    public float spawnRangeMultiplier = 1f; // Multiplicador para ajustar el rango de spawn

    private Queue<string> remainingWords; // Cola para manejar las palabras disponibles

    private void Start()
    {
        // Inicializar la cola con las palabras mezcladas
        ShuffleAndInitializeWords();

        // Comenzar a generar enemigos
        InvokeRepeating("SpawnEnemy", 0f, spawnInterval);
    }

    /// <summary>
    /// Mezcla las palabras y las prepara para ser usadas.
    /// </summary>
    private void ShuffleAndInitializeWords()
    {
        List<string> shuffledWords = new List<string>(wordList);
        for (int i = 0; i < shuffledWords.Count; i++)
        {
            int randomIndex = Random.Range(i, shuffledWords.Count);
            string temp = shuffledWords[i];
            shuffledWords[i] = shuffledWords[randomIndex];
            shuffledWords[randomIndex] = temp;
        }
        remainingWords = new Queue<string>(shuffledWords);
    }

    private void SpawnEnemy()
    {
        // Detener la generación si no quedan palabras
        if (remainingWords.Count == 0)
        {
            CancelInvoke("SpawnEnemy");
            return;
        }

        // Obtener la siguiente palabra de la cola
        string nextWord = remainingWords.Dequeue();

        // Generar posición aleatoria
        Vector3 randomPosition = GetPositionRange();

        // Instanciar el enemigo en la posición aleatoria
        GameObject enemy = Instantiate(enemyPrefab, randomPosition, Quaternion.identity);

        // Asignar una palabra al enemigo
        Enemy enemyCustom = enemy.GetComponent<Enemy>();
        if (enemyCustom != null)
        {
            enemyCustom.SetEnemyWord(nextWord);
        }
    }

    public Vector3 GetPositionRange()
    {
        // Calcular el rango de spawn basado en la escala del EnemySpawner
        float rangeX = transform.localScale.x * spawnRangeMultiplier;
        float rangeY = transform.localScale.y * spawnRangeMultiplier;

        // Generar una posición aleatoria dentro del rango
        Vector3 randomPosition = new Vector3(
            transform.position.x + Random.Range(-rangeX, rangeX),
            transform.position.y + Random.Range(-rangeY, rangeY),
            transform.position.z
        );

        return randomPosition;
    }

    public string[] GetWordList()
    {
        return wordList;
    }
}