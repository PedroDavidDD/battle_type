using UnityEngine;

public class EnemySoundController : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip sonidoGolpeEnemigo;
    [SerializeField] private AudioClip sonidoMuerteEnemigo;

    public void PlayGolpeEnemigoSound()
    {
        if (audioSource != null && sonidoGolpeEnemigo != null)
        {
            audioSource.PlayOneShot(sonidoGolpeEnemigo);
        }
    }

    public void PlayMuerteEnemigoSound()
    {
        if (audioSource != null && sonidoMuerteEnemigo != null)
        {
            audioSource.PlayOneShot(sonidoMuerteEnemigo);
        }
    }
}
