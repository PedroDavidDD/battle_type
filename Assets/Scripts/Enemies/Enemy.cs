using System;
using UnityEngine;
using UnityEngine.Windows;

public class Enemy : MonoBehaviour
{
    [SerializeField] private string word; // Palabra asignada al 
    public float speed = 2f; // Velocidad de movimiento
    private WordManager wordManager;
    [SerializeField] private int enemyLive = 1;

    [System.Obsolete]
    private void Start()
    {
        // Obtener la referencia al WordManager
        wordManager = FindObjectOfType<WordManager>();

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

        enemyLive = word.Length;
    }    
    private void OnDestroy()
    {
        // Desregistrar el enemigo cuando se destruya
        if (wordManager != null)
        {
            wordManager.UnregisterEnemy(this);
        }
    }

    public string GetEnemyWord()
    {
        return this.word;
    }

    public void SetEnemyWord(string word)
    {
        this.word = word;
    }
}