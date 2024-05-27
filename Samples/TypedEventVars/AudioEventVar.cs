//alex@bardicbytes.com

using UnityEngine;

namespace BardicBytes.EventVars.Samples
{
    /// <summary>
    /// Data necessary to configure an audio source for the given clip
    /// </summary>
    [System.Serializable]
    public struct AudioEventData
    {
        public AudioClip clip;
        public float volume;
        public float minPitch;
        public float maxPitch;

        public float GetRandomPitch()
        {
            return Random.Range(minPitch,maxPitch);
        }
    }

    public struct AudioEventHandle
    {
        public AudioEventVar source;
        public AudioEventData data;
        public AudioSource audioSource;
        public bool IsPlaying => audioSource.isPlaying;
        public AudioClip SourceClip => source.Value.clip;
    }

    /// <summary>
    /// By configuring the initial value of asset instances of AudioEvent,
    /// those files can be used to trigger sound effects directly.
    /// Alternatively, unique AudioEventData can be passed to the audio event
    /// through the Raise(AudioEventData) method
    /// </summary>
    [CreateAssetMenu(menuName = "BardicBytes/EventVars/AudioEventVar")]
    public class AudioEventVar : EventVar<AudioEventData>
    {
        public override AudioEventData GetTypedValue(EventVarInstanceData data) => (AudioEventData)data.SystemObjectValue;

        public virtual void Play() => Raise();
    }
}