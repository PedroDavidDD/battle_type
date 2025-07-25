using System;
using UnityEngine;
using UnityEngine.Windows;
using static UnityEngine.EventSystems.EventTrigger;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private string word; // Palabra asignada al enemigo
    public float speed = .3f; // Velocidad de movimiento
    private WordManager wordManager;
    private GameManager gameManager;
    [SerializeField] private int enemyLive = 1;
    
    [SerializeField] private EnemySoundController enemySoundController;

    public int EnemyLive { get => enemyLive; set => enemyLive = value; }

    [SerializeField] private Text enemyTextWord;
    [SerializeField] private Slider liveEnemy;

    [System.Obsolete]
    private void Start()
    {
        // Obtener referencias
        wordManager = GameObject.FindFirstObjectByType<WordManager>();
        gameManager = GameObject.FindFirstObjectByType<GameManager>();

        if (enemySoundController == null)
        {
            enemySoundController = GameObject.FindFirstObjectByType<EnemySoundController>();
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

        if (enemyTextWord != null && liveEnemy != null)
        {
            // Actualizar el texto de la palabra y la barra de vida del enemigo

            enemyTextWord.text = GetEnemyWord();

            liveEnemy.maxValue = GetEnemyWord().Length;
            liveEnemy.value = EnemyLive;
            Debug.Log("Enemigo registrado en EnemyTextWord y liveEnemy.");
        }
        else
        {
            Debug.LogError("Error: EnemyTextWord o liveEnemy no encontrados.");
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