using UnityEngine;

public class EnemyRepeat : MonoBehaviour
{
    // Posici�n espec�fica a la que se mover�n los enemigos cuando toquen este muro
    public GameObject spawnerPosition;

    private void Start()
    {
        // Buscar el GameObject "EnemySpawner" en la escena
        spawnerPosition = GameObject.Find("EnemySpawner");

        // Verificar si se encontr� el GameObject
        if (spawnerPosition == null)
        {
            Debug.LogError("Error: No se encontro el GameObject 'EnemySpawner'.");
        }
    }

    // Detectar colisiones con otros objetos
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Verificar si el objeto colisionado es un enemigo
        if (collision != null && collision.gameObject.CompareTag("Enemy"))
        {
            // Obtener el componente Transform del enemigo
            Transform enemyTransform = collision.transform;

            // Obtener el script EnemySpawner del GameObject "EnemySpawner"
            EnemySpawner enemySpawner = spawnerPosition.GetComponent<EnemySpawner>();
            GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

            // Verificar si el script EnemySpawner existe
            if (enemySpawner != null && gameManager != null)
            {
                // Llamar al m�todo GetPositionRange() desde el script EnemySpawner
                Vector3 randomPosition = enemySpawner.GetPositionRange();

                // Mover al enemigo a la posici�n aleatoria generada
                enemyTransform.position = randomPosition;


                gameManager.LoseLife();
            }
            else
            {
                Debug.LogError("Error: El GameObject 'EnemySpawner' no tiene el script EnemySpawner adjunto.");
            }
        }
    }
}