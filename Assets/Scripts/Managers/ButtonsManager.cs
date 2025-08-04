using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public class ButtonsManager : MonoBehaviour
{    
    public CategoryState categoryState;

    /*
    public static ButtonsManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }*/

    private void Start()
    {
        categoryState = CategoryState.Instance;
    }
    
    // Método que selecciona la categoría (programación o razas de animales de fantasía)
    // y carga la escena del juego con la categoría seleccionada
    public void SelectCategory(string category)
    {
        // Asignar la categoría seleccionada al ButtonsManager
        if (category == "programacion" || category == "razas" || category == "custom")
        {
            categoryState.txtCategory = category;
        }
        else
        {
            categoryState.txtCategory = "programacion";
        }

        SceneManager.LoadScene(2);
    }

    public void Credits()
    {
        SceneManager.LoadScene(3);
    }   
    
    public void OpenFileBrowser()
    {
        // Abrir el diálogo para seleccionar archivo JSON
        string path = UnityEditor.EditorUtility.OpenFilePanel("Seleccionar JSON", "", "json");
        
        if (!string.IsNullOrEmpty(path))
        {
            // Guardar la ruta del archivo seleccionado
            categoryState.customJsonPath = path;
            
            // Cargar la escena del juego (asegúrate de que la escena del juego tiene el índice 2)
            categoryState.txtCategory = "custom"; // Usamos una categoría especial para JSON personalizado
            SceneManager.LoadScene(2);
        }
    }
}
