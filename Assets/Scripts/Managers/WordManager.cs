using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class WordManager : MonoBehaviour
{
    private List<Enemy> activeEnemies = new List<Enemy>();

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private float bulletSpacing = 0.5f;
    
    // Rastrea el progreso por palabra
    private Dictionary<string, int> processedCharacters = new Dictionary<string, int>();
    private string selectedWord = ""; // Palabra seleccionada

    public void SetSelectedWord(string word)
    {
        if (selectedWord != word)
        {
            selectedWord = word;

            processedCharacters[selectedWord] = processedCharacters.ContainsKey(selectedWord)
                ? processedCharacters[selectedWord]
                : 0;
            Debug.Log($"Palabra seleccionada: {selectedWord}");
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
            if (currentInput.Length <= enemy.GetEnemyWord().Length && enemy.GetEnemyWord().StartsWith(currentInput))
            {                
                ForCharacters(currentInput, enemy);
                // ForWord(input, enemyWord);
            }
        }
        LogDictionaryContents();
    }
    private void ForCharacters(string input, Enemy enemy)
    {
        int matchCount = 0;

        // Obtener el progreso actual para la palabra seleccionada
        int processedCount = processedCharacters.ContainsKey(selectedWord) ? processedCharacters[selectedWord] : 0;

        for (int i = processedCount; i < input.Length && i < selectedWord.Length; i++)
        {
            if (input[i] == selectedWord[i])
            {
                matchCount++;
            }
            else
            {
                break;
            }
        }
        // car - caro
        // Disparar una bala por cada carácter coincidente
        if (matchCount > 0)
        {
            // ShootBullet(i);
            enemy.ReduceLive(matchCount);
            // Actualizar el progreso de la palabra seleccionada
            processedCharacters[selectedWord] += matchCount;
        }

    }
    // Dispara una bala en una posición específica.
    private void ShootBullet(int index)
    {
        Vector3 spawnPosition = shootPoint.position + Vector3.right * index * bulletSpacing;
        Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);
    }

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
        GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
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