using UnityEngine;

public class AudioControllerTyping : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip tecla;

    public void PlayTeclaSound()
    {
        if (audioSource != null && tecla != null)
        {
            audioSource.PlayOneShot(tecla);
        }
    }
}
