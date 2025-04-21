using UnityEngine;

public class Enemy : MonoBehaviour
{
    public TextWord textWord;
    public string word; // Palabra asignada al 
    public float speed = 2f; // Velocidad de movimiento
    private WordManager wordManager;


    [System.Obsolete]
    private void Start()
    {
        // Obtener la referencia al WordManager
        wordManager = FindObjectOfType<WordManager>();
        textWord = GetComponent<TextWord>();

        if (wordManager != null)
        {
            wordManager.RegisterEnemy(this); // Registrar este enemigo en el WordManager
            Debug.Log("Enemigo registrado en WordManager.");
        }
        else
        {
            Debug.LogError("Error: WordManager no encontrado.");
        }
    }
    private void Update()
    {
        // Mover el enemigo hacia abajo
        transform.Translate(Vector3.down * speed * Time.deltaTime);
        if (textWord != null) {
            word = textWord.GetTextWord();
            Debug.Log("Palabra INICIALIZADa: " + word);
        }
        else
        {
            Debug.Log("Palabra NO INICIALIZADA: " + textWord);
        }

    }

    public void CheckWord(string input)
    {
        if (input == word)
        {
            Debug.Log("Palabra correcta. Destruyendo enemigo: " + word);
            Destroy(gameObject); // Destruir el enemigo
        }
        else
        {
            Debug.Log(input+" Palabra INCORRECTA: " + word);
        }
    }

    private void OnDestroy()
    {
        // Desregistrar el enemigo cuando se destruya
        if (wordManager != null)
        {
            wordManager.UnregisterEnemy(this);
        }
    }

    public string GetWord()
    {
        return word;
    }
}