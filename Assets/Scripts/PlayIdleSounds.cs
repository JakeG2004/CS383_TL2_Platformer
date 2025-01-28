using UnityEngine;
using System.Collections;

public class PlayIdleSounds : MonoBehaviour
{
    public AudioSource as_Idle;
    public AudioClip[] IdleSounds;

    private void Start()
    {
        StartCoroutine(PlayIdleSoundRoutine());
    }

    private IEnumerator PlayIdleSoundRoutine()
    {
        while (true)
        {
            float waitTime = Random.Range(5f, 15f);
            yield return new WaitForSeconds(waitTime);

            if (IdleSounds.Length > 0 && as_Idle != null)
            {
                int randomIndex = Random.Range(0, IdleSounds.Length);
                AudioClip selectedClip = IdleSounds[randomIndex];
                as_Idle.PlayOneShot(selectedClip);
            }
        }
    }
}
