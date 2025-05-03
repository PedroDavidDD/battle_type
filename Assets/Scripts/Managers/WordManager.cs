using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class WordManager : MonoBehaviour
{
    private List<Enemy> activeEnemies = new List<Enemy>();

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private float bulletSpacing = 0.5f;

    public void RegisterEnemy(Enemy enemy)
    {
        activeEnemies.Add(enemy);
    }

    public void UnregisterEnemy(Enemy enemy)
    {
        activeEnemies.Remove(enemy);
    }

    public void CheckInput(string input)
    {
        Debug.Log("Enviado WorlManager CheckInput: "+ input);
        foreach (var enemy in activeEnemies)
        {
            CheckWord(input, enemy.GetEnemyWord());
            Debug.Log("WorlManager: "+ input);
        }
    }
    public void CheckWord(string input, string enemyWord)
    {
        ForCharacters(input, enemyWord);
        // ForWord(input, enemyWord);
    }
    private void ForCharacters(string input, string enemyWord)
    {
        int matchCount = 0;

        for (int i = 0; i < input.Length && i < enemyWord.Length; i++)
        {
            if (input[i] == enemyWord[i])
            {
                matchCount++;
            }
            else
            {
                break;
            }
        }

        // Disparar una bala por cada carácter coincidente
        for (int i = 0; i < matchCount; i++)
        {
            // ShootBullet(i);
            Debug.Log("disparo: " + i);
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

}