using UnityEngine;

public class EnemySoundController : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip sonidoMuerteEnemigo;

    public void PlayMuerteEnemigoSound()
    {
        if (audioSource != null && sonidoMuerteEnemigo != null)
        {
            audioSource.PlayOneShot(sonidoMuerteEnemigo);
        }
    }
}
