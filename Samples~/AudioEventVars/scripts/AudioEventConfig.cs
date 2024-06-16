//alex@bardicbytes.com

using UnityEngine;
using UnityEngine.Audio;

namespace BardicBytes.EventVars.AudioEventVar
{
    /// <summary>
    /// Data necessary to configure an audio source for the given clip
    /// </summary>
    [System.Serializable]
    public class AudioEventConfig
    {
        public enum ClipSelectionMode
        {
            RandomEachTime = 0,
            RandomNoRepeat = 1,
            RandomOrder = 2,
            OriginalOrder = 3,
        }

        public AudioClip[] clips;

        public ClipSelectionMode selectionMode;

        public bool overrideGroup;
        public AudioMixerGroup group;

        public bool overrideVolume;
        public float volume;

        [Space]
        public bool bypassEffects;
        public bool bypassListenerEffects;

        public float minPitch;
        public float maxPitch;
        public float attenuationMinDistance;

        public System.Action<AudioSource> onClipPlayStart;

        public AudioEventPlaybackConfig GetRequest(int lastIndexPlayed)
        {
            return new AudioEventPlaybackConfig()
            {
                clip = GetNextClip(lastIndexPlayed),
                group = this.group,
                bypassEffects = this.bypassEffects,
                bypassListenerEffects = this.bypassListenerEffects,
                volume = this.volume,
                pitch = Random.Range(minPitch, maxPitch),
                minDistance = attenuationMinDistance,
                maxDistance = 500f
            };
        }

        private AudioClip GetNextClip(int lastIndex)
        {
            AudioClip selected = null;
            switch (this.selectionMode)
            {
                case ClipSelectionMode.RandomEachTime:
                    selected = clips[Random.Range(0, clips.Length)];
                    break;
                case ClipSelectionMode.RandomNoRepeat:
                    //same as random order for now
                    break;
                case ClipSelectionMode.RandomOrder:
                    break;
                case ClipSelectionMode.OriginalOrder:
                    break;
            }
            return selected;
        }
    }
}