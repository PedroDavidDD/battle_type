using UnityEngine;
using UnityEngine.UI;

public class WordController : MonoBehaviour
{
    public string currentWord = ""; // Palabra asignada al enemigo
    public float speed = 2f; // Velocidad de movimiento
    public TextWord textWord;
    private void Start()
    {
        textWord = GetComponent<TextWord>();
    }

    public void SetWord(string word)
    {
        currentWord = word;
        Debug.Log("SET: " + textWord);
        if (textWord != null)
        {
            textWord.UpdateTextWord(word);
            Enemy enemyT = GetComponent<Enemy>();
            enemyT.word = word;
            Debug.Log("SET WORD");
        }
    }

    public void CheckWord(string input)
    {
        if (input == currentWord)
        {
            //GameManager.Instance.AddScore(10); // Aumentar puntuación
            Destroy(gameObject); // Destruir el enemigo
        }
    }   // Crear el texto para mostrar la palabra
   

}