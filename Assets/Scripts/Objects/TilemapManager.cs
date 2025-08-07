using UnityEngine;

public class TilemapController : MonoBehaviour
{
    [Header("Velocidad del scroll")]
    [SerializeField] private float scrollSpeed = 1f;
    [Header("Altura de un solo tilemap (La camara)")]
    [SerializeField] private float tileHeight = 10f;
    
    private Vector3 startPosition;
    private Transform tilemapTransform;

    void Start()
    {
        tilemapTransform = GetComponent<Transform>();
        startPosition = tilemapTransform.position;
    }

    void Update()
    {
        // Mover el tilemap hacia abajo
        float newPosition = Mathf.Repeat(Time.time * scrollSpeed, tileHeight);
        tilemapTransform.position = startPosition + Vector3.down * newPosition;
    }
}
