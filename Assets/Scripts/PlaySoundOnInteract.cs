using UnityEngine;

public class PlaySoundOnInteract : MonoBehaviour 
{
    public AudioSource AudioSource;
    public AudioClip SoundEffect;

    void OnTriggerEnter2D(Collider2D other) { 
    if (other.tag == "Player")
        {
            AudioSource.clip = SoundEffect;
            AudioSource.Play();
        }
    }
}
