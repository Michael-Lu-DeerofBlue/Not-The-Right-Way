using UnityEngine;

public class ExtraSoundController : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip Sound; // Drag your break sound here in the inspector

    void Start()
    {
        // Ensure there's an AudioSource component
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void PlaySound()
    {
        if (Sound != null)
        {
            audioSource.clip = Sound;
            audioSource.Play();
        }
        else
        {
            Debug.LogError("Break sound clip not set");
        }
    }

    // Add more methods for different sounds as needed
}
