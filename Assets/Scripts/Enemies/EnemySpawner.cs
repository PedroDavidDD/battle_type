using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Text;
using System.Globalization;

// Clase estática para manejar la normalización de texto
public static class TextNormalizer
{
    // Variable para activar/desactivar la normalización
    public static bool NormalizationEnabled { get; set; } = true;

    public static string Normalize(string input)
    {
        if (string.IsNullOrEmpty(input) || !NormalizationEnabled)
            return input;

        // Convertir a minúsculas
        string normalized = input.ToLowerInvariant();
        
        // Normalizar la cadena (separar caracteres de sus acentos)
        normalized = normalized.Normalize(NormalizationForm.FormD);
        
        // Eliminar diacríticos, números y símbolos
        var stringBuilder = new StringBuilder();
        foreach (char c in normalized)
        {
            // Mantener letras y espacios
            if (char.IsLetter(c) || c == ' ')
            {
                // Quitar diacríticos
                string normalizedChar = c.ToString()
                    .Normalize(NormalizationForm.FormC);
                stringBuilder.Append(normalizedChar);
            }
            // Reemplazar guiones y otros separadores por espacio
            else if (c == '-' || c == '_' || c == '/')
            {
                stringBuilder.Append(' ');
            }
        }
        
        // Limpiar espacios extras
        string result = string.Join(" ", 
            stringBuilder.ToString()
                .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
        );

        return result;
    }
}

[System.Serializable]
public class WordItem
{
    public string word;
    public string tag;
    [NonSerialized] public string normalizedWord; // No se guarda en el JSON

    public void Normalize()
    {
        normalizedWord = TextNormalizer.Normalize(word);
    }
}

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Sprite[] enemySprites;
    public float spawnInterval = 4f;
    public float spawnRangeMultiplier = 1f;

    public TextAsset defaultWordListFile; // JSON por defecto
    private TextAsset currentWordListFile; // JSON actual (puede ser el default o uno personalizado)
    private WordItem[] wordList;
    private Queue<WordItem> remainingWords;
    private List<WordItem> allWordsCache; // Cache de todas las palabras sin filtrar
    private GameManager gameManager;

    // UI References
    public TMPro.TMP_Text gameStatusText;
    
    string selectedCategory;

    private void Start()
    {

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        
        // Usar el JSON por defecto al inicio
        currentWordListFile = defaultWordListFile;
        InitializeGame();

        // Al iniciar, establecer el total de enemigos en GameManager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.totalEnemiesToSpawn = wordList.Length;
        }
    }
    // Verificar Victoria 
    public void CheckVictory()
    {
        if (gameManager == null) return;

            // Terminar el juego con mensaje de victoria
            StartCoroutine(UpdateGameStatus("¡Ganaste!"));
            CancelInvoke("SpawnEnemy"); // Detener el spawn de enemigos
            gameManager.ShowGameOver();
    }

    private void InitializeGame()
    {
        if (ButtonsManager.Instance == null)
        {
            Debug.LogError("ButtonsManager no encontrado");
            return;
        }

        // Verificar si hay un JSON personalizado cargado
        if (!string.IsNullOrEmpty(ButtonsManager.Instance.customJsonPath))
        {
            try
            {
                string jsonContent = System.IO.File.ReadAllText(ButtonsManager.Instance.customJsonPath);
                TextAsset customJson = CreateTextAsset(jsonContent);
                
                // Primero intentar cargar el JSON personalizado
                if (LoadAndParseJson(customJson))
                {
                    // Verificar si hay palabras con la etiqueta 'custom' o 'programacion'
                    var availableCategories = allWordsCache.Select(w => w.tag.ToLower()).Distinct().ToList();
                    
                    // Si hay palabras con la etiqueta 'custom', usarlas
                    if (availableCategories.Contains("custom"))
                    {
                        selectedCategory = "custom";
                    }
                    // Si no, usar 'programacion' como categoría por defecto
                    else if (availableCategories.Contains("programacion"))
                    {
                        selectedCategory = "programacion";
                    }
                    // Si no hay ninguna de las dos, usar la primera categoría disponible
                    else if (availableCategories.Count > 0)
                    {
                        selectedCategory = availableCategories[0];
                    }
                    
                    Debug.Log($"JSON personalizado cargado correctamente. Categoría seleccionada: {selectedCategory}");
                    
                    // Filtrar por categoría
                    wordList = allWordsCache
                        .Where(item => item.tag.ToLower() == selectedCategory)
                        .ToArray();
                        
                    if (wordList.Length > 0)
                    {
                        if (ShuffleAndInitializeWords())
                        {
                            InvokeRepeating("SpawnEnemy", 0f, spawnInterval);
                            StartCoroutine(UpdateGameStatus("Juego en progreso..."));
                        }
                        return;
                    }
                }
                
                // Si llegamos aquí, hubo un error o no hay palabras en la categoría
                Debug.LogError("Fallo al cargar JSON personalizado o no hay palabras en la categoría, cargando por defecto");
                LoadDefaultWordList();
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error al cargar el archivo JSON personalizado: {e.Message}");
                LoadDefaultWordList();
            }
        }
        else
        {
            // Cargar JSON por defecto si no hay personalizado
            LoadDefaultWordList();
        }

        // Filtrar por categoría
        wordList = allWordsCache
            .Where(item => item.tag.ToLower() == selectedCategory)
            .ToArray();

        if (wordList.Length == 0)
        {
            Debug.LogError($"No hay palabras para la categoría: {selectedCategory}");
            StartCoroutine(UpdateGameStatus("No hay palabras para esta categoría"));
            return;
        }

        if (ShuffleAndInitializeWords())
        {
            InvokeRepeating("SpawnEnemy", 0f, spawnInterval);
            StartCoroutine(UpdateGameStatus("Juego en progreso..."));
        }
    }

    [System.Serializable]
    private class WordListWrapper
    {
        public List<WordItem> items = new List<WordItem>();
    }

    // La clase TextNormalizer ha sido movida al nivel del namespace para mejor accesibilidad

    private bool LoadAndParseJson(TextAsset jsonFile)
    {
        if (jsonFile == null || string.IsNullOrEmpty(jsonFile.text))
        {
            Debug.LogError("El archivo JSON está vacío o es nulo");
            return false;
        }

        try
        {
            var wrapper = JsonUtility.FromJson<WordListWrapper>(jsonFile.text);
            if (wrapper != null && wrapper.items != null && wrapper.items.Count > 0)
            {
                allWordsCache = new List<WordItem>(wrapper.items);
                
                // Normalizar todas las palabras
                foreach (var wordItem in allWordsCache)
                {
                    wordItem.Normalize();
                }
                
                string estadoNormalizacion = TextNormalizer.NormalizationEnabled ? "activada" : "desactivada";
                Debug.Log($"Se cargaron {allWordsCache.Count} palabras del JSON (Normalización: {estadoNormalizacion})");
                return true;
            }
            else
            {
                Debug.LogError("El archivo JSON no contiene palabras válidas o está vacío");
                return false;
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error parsing JSON: {e.Message}");
        }
        return false;
    }

    private bool ShuffleAndInitializeWords()
    {
        if (wordList == null || wordList.Length == 0) return false;

        // Mezcla eficiente con Fisher-Yates
        var shuffledWords = wordList.ToList();
        for (int i = shuffledWords.Count - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            var temp = shuffledWords[i];
            shuffledWords[i] = shuffledWords[j];
            shuffledWords[j] = temp;
        }

        remainingWords = new Queue<WordItem>(shuffledWords);
        return true;
    }

    private void SpawnEnemy()
    {
        // Si no hay más palabras, verificar victoria
        if (remainingWords.Count == 0)
        {
            if (GameManager.Instance != null && 
            GameManager.Instance.enemiesKilled >= GameManager.Instance.totalEnemiesToSpawn)
            {
                CheckVictory();
            }
            return;
        }

        var nextWord = remainingWords.Dequeue();
        var enemy = Instantiate(enemyPrefab, GetPositionRange(), Quaternion.identity);

        // Asignar sprite
        var body = enemy.transform.Find("Body");
        if (body != null && enemySprites.Length > 0)
        {
            body.GetComponent<SpriteRenderer>().sprite = 
                enemySprites[UnityEngine.Random.Range(0, enemySprites.Length)];
        }

        // Asignar palabra (usar la versión normalizada para mostrar)
        var enemyScript = enemy.GetComponent<Enemy>();
        if (enemyScript != null) 
        {
            // Usar la versión normalizada si existe, de lo contrario usar la original
            string wordToShow = !string.IsNullOrEmpty(nextWord.normalizedWord) ? 
                nextWord.normalizedWord : 
                TextNormalizer.Normalize(nextWord.word);
                
            enemyScript.SetEnemyWord(wordToShow);
            
            // Para depuración: mostrar la palabra original y la normalizada
            Debug.Log($"Palabra original: '{nextWord.word}' → Normalizada: '{wordToShow}'");
        }
    }

    private IEnumerator UpdateGameStatus(string message)
    {
        if (gameStatusText != null) gameStatusText.text = message;
        yield return new WaitForSeconds(2f);
        gameStatusText.text = "";
    }

    // Método auxiliar para cargar la lista de palabras por defecto
    private void LoadDefaultWordList()
    {
        if (defaultWordListFile == null)
        {
            Debug.LogError("No se ha asignado un archivo de lista de palabras por defecto");
            return;
        }

        selectedCategory = "custom";
        
        // Solo parsear el JSON si es necesario (primera vez o nuevo archivo)
        if (allWordsCache == null || currentWordListFile != defaultWordListFile)
        {
            if (!LoadAndParseJson(defaultWordListFile))
            {
                Debug.LogError("Fallo al cargar el archivo de palabras por defecto");
                return;
            }
            currentWordListFile = defaultWordListFile;
        }
    }

    private TextAsset CreateTextAsset(string content)
    {
        // Crear un TextAsset virtual (en runtime)
        // Nota: En build real necesitarías una solución diferente
        var ta = new TextAsset(content);
        return ta;
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

}