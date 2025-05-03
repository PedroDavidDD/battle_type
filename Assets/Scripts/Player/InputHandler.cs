using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private string currentInput = "";
    public WordManager wordManager;

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
                    Debug.Log("New Current input: " + currentInput);
                }
            }
            else if (c == '\n' || c == '\r') // Enter (confirmar)
            {
                // wordManager.CheckInput(currentInput);
                Debug.Log("Enter input: "+currentInput);
                currentInput = "";
            }
            else
            {
                currentInput += c;
                Debug.Log("Libre input: " + c);
                wordManager.CheckInput(currentInput);
            }
        }
    }

    public string GetCurrentInput()
    {
        return currentInput;
    }
}