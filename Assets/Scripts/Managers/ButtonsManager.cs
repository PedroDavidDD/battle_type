using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsManager : MonoBehaviour
{
    public static ButtonsManager Instance { get; private set; }
    
    public string txtCategory;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Hace que el objeto persista entre escenas
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void Salir()
    {
        Application.Quit();
    }

    public void Menu()
    {
        SceneManager.LoadScene(0);
    }

    public void Categories()
    {
        SceneManager.LoadScene(1);
    }

    // Método que selecciona la categoría (programación o razas de animales de fantasía)
    // y carga la escena del juego con la categoría seleccionada
    public void SelectCategory(string category)
    {
        // Asignar la categoría seleccionada al ButtonsManager
        if (category == "programacion" || category == "razas")
        {
            txtCategory = category;
        }
        else
        {
            txtCategory = "programacion";
        }

        SceneManager.LoadScene(2);
    }

    public void Credits()
    {
        SceneManager.LoadScene(3);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene( SceneManager.GetActiveScene().name );
    }
}
