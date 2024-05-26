//alex@bardicbytes.com

using BardicBytes.EventVars;
using BardicBytes.EventVars.Samples;
using UnityEngine;

public class AudioEventPlayer : EventVarListener<AudioEventData>
{
    [SerializeField]
    protected AudioSource audioSource;

    protected override void HandleTypedEventRaised(AudioEventData data)
    {
        ConfigureAudioSource(data);

        Play();

        base.HandleTypedEventRaised(data);
    }

    protected virtual void ConfigureAudioSource(AudioEventData data)
    {
        audioSource.clip = data.clip;
        audioSource.volume = data.volume;
        audioSource.pitch = data.GetRandomPitch();
    }

    /// <summary>
    /// plays the audio source.
    /// </summary>
    protected virtual void Play()
    {
        audioSource.Play();
    }
}
