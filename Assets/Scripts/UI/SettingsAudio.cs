using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsAudio : MonoBehaviour
{
    [Header("Audio Mixer Groups")]
    [SerializeField] private AudioMixer audioMixer;
    
    [Header("Volume Parameters")]
    [SerializeField] private string masterVolumeParam = "MasterVolume";
    [SerializeField] private string typingVolumeParam = "TypingVolume";
    [SerializeField] private string othersVolumeParam = "OthersVolume";

    
    [Header("UI Sliders")]
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider typingVolumeSlider;
    [SerializeField] private Slider othersVolumeSlider;
    
    private const float _minVolume = -80f;
    private const float _maxVolume = 0f;

    private void Start()
    {
        // Initialize sliders with saved values or defaults
        float savedMasterVolume = PlayerPrefs.GetFloat(masterVolumeParam, 0.8f);
        float savedTypingVolume = PlayerPrefs.GetFloat(typingVolumeParam, 1f);
        float savedOthersVolume = PlayerPrefs.GetFloat(othersVolumeParam, 1f);        
        if (masterVolumeSlider != null)
        {
            masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
            masterVolumeSlider.value = savedMasterVolume;
        }
        
        if (typingVolumeSlider != null)
        {
            typingVolumeSlider.onValueChanged.AddListener(SetTypingVolume);
            typingVolumeSlider.value = savedTypingVolume;
        }
        
        if (othersVolumeSlider != null)
        {
            othersVolumeSlider.onValueChanged.AddListener(SetOthersVolume);
            othersVolumeSlider.value = savedOthersVolume;
        }
        
        // Apply saved volumes
        SetMasterVolume(savedMasterVolume);
        SetTypingVolume(savedTypingVolume);
        SetOthersVolume(savedOthersVolume);
    }

    public void SetMasterVolume(float volume)
    {
        // Convert 0-1 range to decibels
        float volumeDB = Mathf.Lerp(_minVolume, _maxVolume, volume);
        audioMixer.SetFloat(masterVolumeParam, volumeDB);
        
        // Save preference
        PlayerPrefs.SetFloat(masterVolumeParam, volume);
    }

    public void SetTypingVolume(float volume)
    {
        // Convert 0-1 range to decibels
        float volumeDB = Mathf.Lerp(_minVolume, _maxVolume, volume);
        audioMixer.SetFloat(typingVolumeParam, volumeDB);
        
        // Save preference
        PlayerPrefs.SetFloat(typingVolumeParam, volume);
    }

    public void SetOthersVolume(float volume)
    {
        // Convert 0-1 range to decibels
        float volumeDB = Mathf.Lerp(_minVolume, _maxVolume, volume);
        audioMixer.SetFloat(othersVolumeParam, volumeDB);
        
        // Save preference
        PlayerPrefs.SetFloat(othersVolumeParam, volume);
    }
    
    private void OnDestroy()
    {
        // Clean up listeners
        if (masterVolumeSlider != null)
            masterVolumeSlider.onValueChanged.RemoveListener(SetMasterVolume);
            
        if (typingVolumeSlider != null)
            typingVolumeSlider.onValueChanged.RemoveListener(SetTypingVolume);
            
        if (othersVolumeSlider != null)
            othersVolumeSlider.onValueChanged.RemoveListener(SetOthersVolume);            
        // Save all preferences
        PlayerPrefs.Save();
    }
}