using UnityEngine;
using UnityEngine.UI;

public class TextWord : MonoBehaviour
{
    [SerializeField] private Text wordText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void UpdateTextWord(string currentWord)
    {
        wordText.text = currentWord; // Asignar la palabra inicial
        Debug.Log("Palabra asignada al texto del enemigo: " + currentWord);

    }
    public string GetTextWord()
    {
        return wordText.text;

    }
}
