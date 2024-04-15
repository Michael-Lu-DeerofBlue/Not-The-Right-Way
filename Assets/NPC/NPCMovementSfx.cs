using UnityEngine;

public class NPCMovementSfx : MonoBehaviour
{
    public AudioClip[] footstepSounds;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Method to be called by the animation frames
    public void PlayFootstepSound()
    {
        int index = Random.Range(0, footstepSounds.Length);
        audioSource.clip = footstepSounds[index];
        audioSource.Play();
    }
}
