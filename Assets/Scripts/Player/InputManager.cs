using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private string currentInput = "";
    private List<WordController> activeEnemies = new List<WordController>();

    private void Update()
    {
        // Detectar entrada del teclado
        foreach (char c in Input.inputString)
        {
            if (c == '\b') // Retroceso (borrar)
            {
                if (currentInput.Length > 0)
                {
                    currentInput = currentInput.Substring(0, currentInput.Length - 1);
                }
            }
            else if (c == '\n' || c == '\r') // Enter (confirmar)
            {
                CheckInput();
            }
            else
            {
                currentInput += c;
            }
        }

        Debug.Log("Current Input: " + currentInput);
    }

    public void RegisterEnemy(WordController enemy)
    {
        activeEnemies.Add(enemy);
    }

    public void UnregisterEnemy(WordController enemy)
    {
        activeEnemies.Remove(enemy);
    }

    private void CheckInput()
    {
        foreach (var enemy in activeEnemies)
        {
            enemy.CheckWord(currentInput);
        }

        currentInput = ""; // Limpiar la entrada después de confirmar
    }
}