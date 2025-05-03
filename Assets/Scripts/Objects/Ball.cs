using UnityEngine;

public class Ball : MonoBehaviour
{
    // Objetivo a seguir
    private Transform target = null; 
    private float speed = 5f;

    private void Update()
    {
        if (target != null)
        {
            // Mover la bala hacia el objetivo
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

            // Si llega al objetivo, destruir la bala
            if (Vector3.Distance(transform.position, target.position) < 0.1f)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            // Si no hay objetivo, destruir la bala
            Destroy(gameObject);
        }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verificar si colisionó con el enemigo asignado
        if (collision.transform == target)
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.ReduceLive(1); 
                Debug.Log($"Bala impactó en el enemigo: {enemy.GetEnemyWord()}");
            }

            Destroy(gameObject);
        }
    }
}