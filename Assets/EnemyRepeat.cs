using UnityEngine;

public class EnemyRepeat : MonoBehaviour
{
    // Posición específica a la que se moverán los enemigos cuando toquen este muro
    public GameObject spawnerPosition;

    private void Start()
    {
        // Buscar el GameObject "EnemySpawner" en la escena
        spawnerPosition = GameObject.Find("EnemySpawner");

        // Verificar si se encontró el GameObject
        if (spawnerPosition == null)
        {
            Debug.LogError("Error: No se encontró el GameObject 'EnemySpawner'.");
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

            // Verificar si el script EnemySpawner existe
            if (enemySpawner != null)
            {
                // Llamar al método GetPositionRange() desde el script EnemySpawner
                Vector3 randomPosition = enemySpawner.GetPositionRange();

                // Mover al enemigo a la posición aleatoria generada
                enemyTransform.position = randomPosition;
            }
            else
            {
                Debug.LogError("Error: El GameObject 'EnemySpawner' no tiene el script EnemySpawner adjunto.");
            }
        }
    }
}