using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager Instance;

    [SerializeField] private AudioSource soundFXObject;
    [SerializeField] private AudioSource soundFXReverbObject;

    private void Awake()
    {
        if(Instance == null)
        Instance = this;
    }

    public void PlaySoundFXClip(AudioClip audioClip, float volume, Transform spawnTransform = null)
    {
        if (spawnTransform == null)
            spawnTransform = transform;

        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        audioSource.clip = audioClip;
        audioSource.volume = volume;

        audioSource.Play();

        float clipLength = audioSource.clip.length;

        Destroy(audioSource.gameObject, clipLength + .4f);
    }
    public void PlaySoundFXReverbClip(AudioClip audioClip, float volume, Transform spawnTransform = null)
    {
        if (spawnTransform == null)
            spawnTransform = transform;

        AudioSource audioSource = Instantiate(soundFXReverbObject, spawnTransform.position, Quaternion.identity);

        audioSource.clip = audioClip;
        audioSource.volume = volume;

        audioSource.Play();

        float clipLength = audioSource.clip.length;

        Destroy(audioSource.gameObject, clipLength + 5);
    }

    public void PlayRandomSoundFXClip(AudioClip[] audioClip, float volume, Transform spawnTransform = null)
    {
        if (spawnTransform == null)
            spawnTransform = transform;

        int i = Random.Range(0, audioClip.Length);

        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        audioSource.clip = audioClip[i];
        audioSource.volume = volume;

        audioSource.Play();

        float clipLength = audioSource.clip.length;

        Destroy(audioSource.gameObject, clipLength);
    }
}
