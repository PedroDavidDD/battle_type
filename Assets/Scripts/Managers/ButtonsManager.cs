using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public class ButtonsManager : MonoBehaviour
{
    public static ButtonsManager Instance { get; private set; }
    
    public string txtCategory;
    public string customJsonPath; // Para almacenar la ruta del JSON personalizado
    
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
        if (category == "programacion" || category == "razas" || category == "custom")
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
        SceneManager.LoadScene( SceneManager.GetActiveScene().buildIndex );
    }

    public void OpenFileBrowser()
    {
        // Abrir el diálogo para seleccionar archivo JSON
        string path = UnityEditor.EditorUtility.OpenFilePanel("Seleccionar JSON", "", "json");
        
        if (!string.IsNullOrEmpty(path))
        {
            // Guardar la ruta del archivo seleccionado
            customJsonPath = path;
            
            // Cargar la escena del juego (asegúrate de que la escena del juego tiene el índice 2)
            txtCategory = "custom"; // Usamos una categoría especial para JSON personalizado
            SceneManager.LoadScene(2);
        }
    }
}
