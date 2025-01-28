using UnityEngine;

public class PlayRandomSFX : MonoBehaviour
{
    public AudioClip[] soundEffects; 
    public AudioSource audioPlayer; 

    void Start()
    {
        if (audioPlayer == null)
        {
            audioPlayer = GameObject.FindGameObjectWithTag("GameSFXHandler").GetComponent<AudioSource>();
        }

        if (soundEffects.Length == 0)
        {
            Debug.LogWarning("No sound effects assigned!");
        }
    }

    public void PlayRandomSound()
    {
        if (soundEffects.Length > 0)
        {
            // Randomly select an index
            int randomIndex = Random.Range(0, soundEffects.Length);

            // Assign the selected clip to the AudioSource
            audioPlayer.clip = soundEffects[randomIndex];

            // Play the sound effect
            audioPlayer.Play();
        }
        else
        {
            Debug.LogWarning("Sound effect array is empty!");
        }
    }
}
