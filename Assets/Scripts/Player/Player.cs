using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] public PlayerSoundController playerSoundController;
    
    public static Player Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (playerSoundController == null)
        { 
            playerSoundController = GameObject.Find("Player").GetComponent<PlayerSoundController>();
        }
    }
}
