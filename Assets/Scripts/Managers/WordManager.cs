using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;

public class WordManager : MonoBehaviour
{
    private List<Enemy> activeEnemies = new List<Enemy>();

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform shootPoint;
    
    // Rastrea el progreso por cada palabra que se registra
    private Dictionary<string, int> processedCharacters = new Dictionary<string, int>();
    // Palabra seleccionada
    private string selectedWord = "";

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
    private void ForCharacters(string input, Enemy enemy)
    {
        int matchCount = 0;

        // Consulta el valor de una clave sin modificar el diccionario 
        int processedCount = processedCharacters.ContainsKey(selectedWord) 
            ? processedCharacters[selectedWord] : 0;

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
        // Disparar una bala por cada carácter coincidente
        if (matchCount > 0)
        {
            ShootBullet(enemy.transform);
            //enemy.ReduceLive(matchCount);
            // Actualizar el progreso de la palabra seleccionada
            processedCharacters[selectedWord] += matchCount;
        }
        Debug.Log("total matchCount " + matchCount);
        Debug.Log("total selectedWord " + selectedWord);

    }
    // Dispara una bala en una posición específica.
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