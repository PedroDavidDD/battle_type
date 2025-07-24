using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Prefab del enemigo
    public Sprite[] enemySprites; // Array de sprites disponibles
    public float spawnInterval = 4f; // Intervalo entre spawns
    public TextAsset wordListFile;
    private string[] wordList;
    public float spawnRangeMultiplier = 1f; // Multiplicador para ajustar el rango de spawn

    private Queue<string> remainingWords; // Cola para manejar las palabras disponibles

    private void Start()
    {
        // Cargar palabras desde el archivo de texto
        if (wordListFile != null)
        {
            // Dividir el texto por saltos de linea y eliminar entradas vacias
            wordList = wordListFile.text.Split(
                new[] { '\r', '\n' },
                System.StringSplitOptions.RemoveEmptyEntries
            );
        }
        else
        {
            Debug.LogError("No se asigno el archivo de palabras.");
        }
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
        if (remainingWords.Count == 0)
        {
            CancelInvoke("SpawnEnemy");
            return;
        }

        string nextWord = remainingWords.Dequeue();
        Vector3 randomPosition = GetPositionRange();
        GameObject enemy = Instantiate(enemyPrefab, randomPosition, Quaternion.identity);

        // --- Buscar el objeto "body" y asignar el sprite ---
        Transform body = enemy.transform.Find("Body"); // Busca el hijo "body"
        if (body != null)
        {
            SpriteRenderer bodyRenderer = body.GetComponent<SpriteRenderer>();
            if (bodyRenderer != null && enemySprites.Length > 0)
            {
                // Asignar un sprite aleatorio al cuerpo
                bodyRenderer.sprite = enemySprites[Random.Range(0, enemySprites.Length)];
            }
            else
            {
                Debug.LogError("El objeto 'body' no tiene SpriteRenderer o no hay sprites asignados.");
            }
        }
        else
        {
            Debug.LogError("No se encontro el objeto hijo 'body' en el prefab del enemigo.");
        }

        // Asignar la palabra al enemigo (codigo existente)
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

        // Generar una posicion aleatoria dentro del rango
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