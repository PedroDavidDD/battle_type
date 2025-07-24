using System;
using UnityEngine;
using UnityEngine.Windows;
using static UnityEngine.EventSystems.EventTrigger;

public class Enemy : MonoBehaviour
{
    [SerializeField] private string word; // Palabra asignada al enemigo
    public float speed = .3f; // Velocidad de movimiento
    private WordManager wordManager;
    private GameManager gameManager;
    [SerializeField] private int enemyLive = 1;
    
    [SerializeField] private EnemySoundController enemySoundController;

    public int EnemyLive { get => enemyLive; set => enemyLive = value; }

    [System.Obsolete]
    private void Start()
    {
        // Obtener referencias
        wordManager = FindObjectOfType<WordManager>();
        gameManager = FindObjectOfType<GameManager>();

        // Obtener el componente EnemySoundController
        if (enemySoundController == null)
        {
            enemySoundController = FindObjectOfType<EnemySoundController>();
        }

        if (wordManager != null)
        {
            // Registrar este enemigo en el WordManager
            wordManager.RegisterEnemy(this); 
            Debug.Log("Enemigo registrado en WordManager.");
        }
        else
        {
            Debug.LogError("Error: WordManager no encontrado.");
        }
        // Asignar la vida del enemigo
        EnemyLive = GetEnemyWord().Length;
    }
    private void Update()
    {
        // Mover el enemigo hacia abajo
        transform.Translate(Vector3.down * speed * Time.deltaTime);
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
    public void ReduceLive(int amount = 1)
    {
        if (gameManager == null) return;

        this.enemyLive -= amount;
        
        enemySoundController.PlayGolpeEnemigoSound();
        
        if (this.enemyLive <= 0)
        {
            Debug.Log("El enemigo ha sido derrotado.");
            Destroy(gameObject);
            EnemyPoints(5);

            enemySoundController.PlayMuerteEnemigoSound();
            
            // Incrementar el contador de enemigos eliminados
            gameManager.AddEnemyKilled();

            InputHandler inputHandler = GameObject.Find("InputHandler").GetComponent<InputHandler>();
            inputHandler.SetCurrentInput("");
        }
    }
    private void EnemyPoints(int points = 10)
    {   
        if (gameManager == null) return;
        gameManager.AddScore(points);
    }
}