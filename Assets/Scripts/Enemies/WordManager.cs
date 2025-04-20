using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class WordManager : MonoBehaviour
{
    private List<Enemy> activeEnemies = new List<Enemy>();

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
            enemy.CheckWord(input);
            Debug.Log("WorlManager: "+ input);
        }
    }
}