using UnityEngine;

public class EnemyRepeat : MonoBehaviour
{
    // Posici�n espec�fica a la que se mover�n los enemigos cuando toquen este muro
    public Vector3 retreatPosition;

    // Detectar colisiones con otros objetos
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Verificar si el objeto colisionado es un enemigo
        if (collision != null && collision.gameObject.CompareTag("Enemy"))
        {
            // Obtener el componente Transform del enemigo
            Transform enemyTransform = collision.transform;

            // Mover al enemigo a la posici�n espec�fica
            enemyTransform.position = retreatPosition;
        }
    }
}