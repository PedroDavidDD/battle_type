using UnityEngine;
using System.Collections;

public class Guia : MonoBehaviour
{
    [SerializeField] private GameObject teclado;

    void Start()
    {
        teclado.SetActive(true);
        // Aqui va una funcion que espera 5 segundos y desactiva la imagen del teclado
        StartCoroutine(WaitAndHideKeyboard());
    }

    private IEnumerator WaitAndHideKeyboard()
    {
        yield return new WaitForSeconds(5);
        teclado.SetActive(false);
    }
}
