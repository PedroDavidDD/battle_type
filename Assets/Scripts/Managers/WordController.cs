using UnityEngine;
using UnityEngine.UI;

public class WordController : MonoBehaviour
{
    [SerializeField] private Enemy enemy;

    [SerializeField] private Text enemyTextWord;
    [SerializeField] private Slider liveEnemy;
    private void Awake()
    {
        enemy = GetComponent<Enemy>();
    }
    private void Update()
    {
        if (enemy == null) return;
        if (enemyTextWord == null) return;
        if (liveEnemy == null) return;

        enemyTextWord.text = enemy.GetEnemyWord();

        liveEnemy.maxValue = enemy.GetEnemyWord().Length;
        liveEnemy.value = enemy.EnemyLive;
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