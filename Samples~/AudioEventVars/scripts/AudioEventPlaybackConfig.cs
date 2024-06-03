//alex@bardicbytes.com

using UnityEngine;
using UnityEngine.Audio;

namespace BardicBytes.EventVars.AudioEventVar
{
    /// <summary>
    /// This struct contains information for configuring an AudioSource to play an AudioClip
    /// </summary>
    [System.Serializable]
    public struct AudioEventPlaybackConfig
    {
        public AudioClip clip;
        public AudioMixerGroup group;

        public bool bypassEffects;
        public bool bypassListenerEffects;

        public bool loop;
        public float volume;
        public float pitch;
        public float minDistance;
        public float maxDistance;

        public Transform target;

        public AudioSource ApplyTo(AudioSource audioSource)
        {
            audioSource.clip = clip;
            audioSource.volume = volume;
            audioSource.outputAudioMixerGroup = group;
            audioSource.loop = loop;
            audioSource.pitch = pitch;
            audioSource.minDistance = minDistance;
            audioSource.maxDistance = maxDistance;
            audioSource.bypassEffects = bypassEffects;
            audioSource.bypassListenerEffects = bypassListenerEffects;

            if (target != null) 
            {
                audioSource.transform.position = target.position;
            }

            return audioSource;
        }
    }
}