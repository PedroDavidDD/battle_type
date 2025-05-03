using UnityEngine;
using UnityEngine.UI;

public class WordController : MonoBehaviour
{
    private TextWord enemyTextWord;
    private Enemy enemy;
    private void Start()
    {
        enemy = GetComponent<Enemy>();
        enemyTextWord = GetComponentInChildren<TextWord>();
    }

    private void Update()
    {
        if (enemy != null && enemyTextWord != null)
        {
            enemyTextWord.UpdateTextWord(enemy.GetEnemyWord());
        }
    }

    public void CheckWord(string input)
    {/*
        if (input == currentWord)
        {
            //GameManager.Instance.AddScore(10); // Aumentar puntuación
            Destroy(gameObject); // Destruir el enemigo
        }*/
    }   // Crear el texto para mostrar la palabra
   

}