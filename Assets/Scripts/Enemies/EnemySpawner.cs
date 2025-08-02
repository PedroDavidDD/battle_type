using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

[System.Serializable]
public class WordItem
{
    public string word;
    public string tag;
}

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Prefab del enemigo
    public Sprite[] enemySprites; // Array de sprites disponibles
    public float spawnInterval = 4f; // Intervalo entre spawns
    public float spawnRangeMultiplier = 1f; // Multiplicador para ajustar el rango de spawn

    public TextAsset wordListFile;
    private WordItem[] wordList;
    private Queue<WordItem> remainingWords;

    private void Start()
    {
        // Verificar si el ButtonsManager existe en la escena
        if (ButtonsManager.Instance == null)
        {
            Debug.LogError("No se encontró el ButtonsManager en la escena");
            return;
        }

        // Obtener la categoría seleccionada
        string selectedCategory = ButtonsManager.Instance.txtCategory?.ToLower() ?? "programacion";
        
        // Cargar palabras desde el archivo de texto
        if (wordListFile != null && !string.IsNullOrEmpty(wordListFile.text))
        {
            try
            {
                // Deserializar el JSON directamente ya que el archivo ya tiene el formato correcto
                var wrapper = JsonUtility.FromJson<WordListWrapper>(wordListFile.text);
                if (wrapper != null && wrapper.items != null)
                {
                    // Filtrar palabras por la categoría seleccionada
                    wordList = wrapper.items
                        .Where(item => item.tag.ToLower() == selectedCategory)
                        .ToArray();
                        
                    Debug.Log($"Categoría seleccionada: {selectedCategory}");
                    Debug.Log($"Palabras encontradas: {wordList.Length}");
                    
                    if (wordList.Length == 0)
                    {
                        Debug.LogError($"No se encontraron palabras para la categoría: {selectedCategory}");
                        return;
                    }
                    // Inicializar la cola con las palabras mezcladas
                    if (ShuffleAndInitializeWords())
                    {
                        // Comenzar a generar enemigos solo si la inicialización fue exitosa
                        InvokeRepeating("SpawnEnemy", 0f, spawnInterval);
                        return;
                    }
                }
                else
                {
                    Debug.LogError("El archivo JSON no tiene el formato correcto o está vacío.");
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Error al parsear el archivo JSON: {e.Message}");
            }
        }
        else
        {
            Debug.LogError("No se asigno el archivo de palabras o está vacío.");
        }

        // Si llegamos aquí, algo salió mal
        wordList = new WordItem[0];
        // Asegurarse de que no se intente generar enemigos
        CancelInvoke("SpawnEnemy");
    }

    /// <summary>
    /// Mezcla las palabras y las prepara para ser usadas.
    /// </summary>
    private bool ShuffleAndInitializeWords()
    {
        if (wordList == null || wordList.Length == 0)
        {
            Debug.LogError("No hay palabras para mezclar.");
            return false;
        }

        List<WordItem> shuffledWords = new List<WordItem>(wordList);
        for (int i = 0; i < shuffledWords.Count; i++)
        {
            int randomIndex = UnityEngine.Random.Range(i, shuffledWords.Count);
            WordItem temp = shuffledWords[i];
            shuffledWords[i] = shuffledWords[randomIndex];
            shuffledWords[randomIndex] = temp;
        }
        if (shuffledWords == null || shuffledWords.Count == 0)
        {
            Debug.LogError("No se pudieron mezclar las palabras.");
            return false;
        }

        remainingWords = new Queue<WordItem>(shuffledWords);
        return true;
    }

    private void SpawnEnemy()
    {
        if (remainingWords.Count == 0)
        {
            CancelInvoke("SpawnEnemy");
            return;
        }

        WordItem nextWord = remainingWords.Dequeue();
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
                bodyRenderer.sprite = enemySprites[UnityEngine.Random.Range(0, enemySprites.Length)];
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

        // Asignar la palabra al enemigo
        Enemy enemyCustom = enemy.GetComponent<Enemy>();
        if (enemyCustom != null)
        {
            enemyCustom.SetEnemyWord(nextWord.word);
            // Aquí podrías usar nextWord.tag si necesitas la etiqueta para algo
        }
    }

    public Vector3 GetPositionRange()
    {
        // Calcular el rango de spawn basado en la escala del EnemySpawner
        float rangeX = transform.localScale.x * spawnRangeMultiplier;
        float rangeY = transform.localScale.y * spawnRangeMultiplier;

        // Generar una posicion aleatoria dentro del rango
        Vector3 randomPosition = new Vector3(
            transform.position.x + UnityEngine.Random.Range(-rangeX, rangeX),
            transform.position.y + UnityEngine.Random.Range(-rangeY, rangeY),
            transform.position.z
        );

        return randomPosition;
    }

    public string[] GetWordList()
    {
        return wordList.Select(item => item.word).ToArray();
    }

    // Clase auxiliar para deserializar el JSON
    [System.Serializable]
    private class WordListWrapper
    {
        public WordItem[] items;
    }
}