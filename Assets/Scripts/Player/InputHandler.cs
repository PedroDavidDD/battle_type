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
                    Debug.Log("Current 1: " + currentInput);
                }
            }
            else if (c == '\n' || c == '\r') // Enter (confirmar)
            {
                wordManager.CheckInput(currentInput);
                Debug.Log("Enviado input: "+currentInput);
                currentInput = ""; // Limpiar la entrada después de confirmar
            }
            else
            {
                currentInput += c;
            }
        }

        Debug.Log("Current Input: " + currentInput);
    }
}