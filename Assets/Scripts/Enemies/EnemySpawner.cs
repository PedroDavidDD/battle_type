using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Prefab del enemigo
    public float spawnInterval = 4f; // Intervalo entre spawns
    public string[] wordList = { "apple", "banana", "car", "dog", "elephant" }; // Lista de palabras

    private void Start()
    {
        InvokeRepeating("SpawnEnemy", 0f, spawnInterval);
    }

    private void SpawnEnemy()
    {
        Vector3 randomPosition = new Vector3(Random.Range(-8f, 8f), 6f, 0f); // Posición aleatoria en la parte superior
        GameObject enemy = Instantiate(enemyPrefab, randomPosition, Quaternion.identity);

        // Asignar una palabra aleatoria al enemigo
        WordController wordController = enemy.GetComponent<WordController>();

        Debug.Log("wordController ."+ wordController);
        if (wordController != null)
        {
            wordController.SetWord(wordList[Random.Range(1, wordList.Length)]);

            Debug.Log("Palabra asignada."+ wordList[Random.Range(1, wordList.Length)]);
        }
        else
        {

            Debug.Log("Palabra NO asignada.");
        }
    }
}