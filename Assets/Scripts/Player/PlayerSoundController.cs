using UnityEngine;

public class PlayerSoundController : MonoBehaviour
{
    // Variables públicas para asignar en el Inspector
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip sonidoDisparo;
    [SerializeField] private AudioClip sonidoMuerteJugador;

    // Método para reproducir el sonido de disparo
    public void PlayDisparoSound()
    {
        if (audioSource != null && sonidoDisparo != null)
        {
            audioSource.PlayOneShot(sonidoDisparo);
        }
    }
        
    public void PlayMuerteJugadorSound()
    {
        if (audioSource != null && sonidoMuerteJugador != null)
        {
            audioSource.PlayOneShot(sonidoMuerteJugador);
        }
    }
}
