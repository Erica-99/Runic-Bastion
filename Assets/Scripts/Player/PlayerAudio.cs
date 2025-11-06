using UnityEditor.PackageManager;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    // References to audio clips and sources
    public AudioClip jumpSoundClip;
    public AudioClip[] stepSoundClips;
    public AudioSource jumpAudioSource;
    public AudioSource stepAudioSource;

    private int prevrand = 0;

    public void PlayJump()
    {
        if (jumpAudioSource != null && jumpSoundClip != null)
        {
            if (!jumpAudioSource.isPlaying)
            {
                jumpAudioSource.PlayOneShot(jumpSoundClip);
            }
        }
        else
        {
            Debug.LogWarning("Jump audio source or clip not assigned!");
        }
    }

    public void PlayStep()
    {
        int randomIndex = GenerateStepIndex();

        if (stepAudioSource && stepSoundClips.Length > 0f)
        {
            stepAudioSource.PlayOneShot(stepSoundClips[randomIndex]);
        }
    }

    private int GenerateStepIndex()
    {
        int randClipIndex = -1;
        do
        {
            float randFloat = Random.value;

            randFloat = Mathf.Clamp(randFloat * stepSoundClips.Length, 0f, 7.99999f);

            randClipIndex = (int)randFloat;
        } while (randClipIndex == prevrand);
        
        prevrand = randClipIndex;

        return randClipIndex;
    }
}