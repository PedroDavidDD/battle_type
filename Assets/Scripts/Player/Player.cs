using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] GameObject ball;
    [SerializeField] bool canShoot = false;
    private AudioSource audioSource;


    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        if (!audioSource)
        {
            audioSource.Play();
        }

        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (ball != null && canShoot)
        {
            
        }
    }

}
