using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    // References to audio clips and sources
    public AudioClip jumpSoundClip;
    public AudioSource jumpAudioSource;
    public AudioClip stepSoundClip;
    public AudioSource stepAudioSource;

    public void PlayJump()
    {
        if (jumpAudioSource != null && jumpSoundClip != null)
        {
            jumpAudioSource.PlayOneShot(jumpSoundClip);
        }
        else
        {
            Debug.LogWarning("Jump audio source or clip not assigned!");
        }
    }

    public void PlayStep()
    {
        if (stepAudioSource && stepSoundClip && !stepAudioSource.isPlaying)
            stepAudioSource.PlayOneShot(stepSoundClip);
    }

    private void Update()
    {
        // Jump sound
        if (Input.GetKeyDown(KeyCode.Space))
            PlayJump();

        if (Input.GetKeyDown(KeyCode.W) ||
            Input.GetKeyDown(KeyCode.A) ||
            Input.GetKeyDown(KeyCode.S) ||
            Input.GetKeyDown(KeyCode.D))
        {
            PlayStep();
        }
    }
}