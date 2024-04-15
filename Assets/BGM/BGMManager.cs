using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance;

    public AudioSource audioSource;

    public AudioClip mainMenuMusic;
    public AudioClip inGameMusic;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Persist if multiple scenes were to be used
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        if (audioSource.clip == clip && audioSource.isPlaying)
            return;  // Avoid restarting the same music if already playing

        audioSource.Stop();  // Stop current music
        audioSource.clip = clip;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void PlayMainMenuMusic()
    {
        PlayMusic(mainMenuMusic);
    }

    public void PlayInGameMusic()
    {
        PlayMusic(inGameMusic);
    }
}
