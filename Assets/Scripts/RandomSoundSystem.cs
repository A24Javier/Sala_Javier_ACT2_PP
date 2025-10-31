using UnityEngine;

public class RandomSoundSystem : MonoBehaviour
{
    [SerializeField] private AudioClip[] sounds;
    [SerializeField] private AudioSource sfxSource;

    public void PlayRandomSound()
    {
        int rand = Random.Range(0, sounds.Length);
        sfxSource.PlayOneShot(sounds[rand]);
    }
}
