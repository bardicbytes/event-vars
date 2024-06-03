//alex@bardicbytes.com

using System.Collections.Generic;
using UnityEngine;

namespace BardicBytes.EventVars.AudioEventVar
{
    /// <summary>
    /// By configuring the initial value of asset instances of AudioEvent,
    /// those files can be used to trigger sound effects directly.
    /// Alternatively, unique AudioEventData can be passed to the audio event
    /// through the Raise(AudioEventData) method
    /// </summary>
    [CreateAssetMenu(menuName = "BardicBytes/EventVars/Audio EventVar")]
    public class AudioEventVar : EventVar<AudioEventConfig, AudioEventPlaybackConfig, AudioEventVar>
    {
        [Tooltip("when true, a given clip will only be played ")]
        [SerializeField]
        private bool _preventSameFramePlays = true;

        private int _lastFramePlayed = -1;
        private int _lastIndexPlayed = -1;

        private List<int> _availableClipIndecies;

        protected override void OnEnable()
        {
            _lastFramePlayed = -1;

            _availableClipIndecies = new List<int>(TypedStoredValue.clips.Length);

            //
            switch (TypedStoredValue.selectionMode)
            {
                case AudioEventConfig.ClipSelectionMode.RandomNoRepeat:
                    Shuffle(_availableClipIndecies);
                    break;
                case AudioEventConfig.ClipSelectionMode.RandomOrder:
                    Shuffle(_availableClipIndecies);
                    break;
                case AudioEventConfig.ClipSelectionMode.OriginalOrder:
                    break;
            }

            base.OnEnable();
        }

        public override AudioEventPlaybackConfig Evaluate(AudioEventConfig inValue) => inValue.GetRequest(_lastIndexPlayed);

        public override AudioEventConfig GetTypedValue(EventVarInstanceData data)
        {
            return (AudioEventConfig)data.SystemObjectValue;
        }

        public virtual void Play() => Raise();

        public override void Raise()
        {
            _lastIndexPlayed = 0;
            if (_preventSameFramePlays && _lastFramePlayed == Time.frameCount)
            {
                return;
            }
            _lastFramePlayed = Time.frameCount;
            base.Raise();
        }

        public void Shuffle<T>(IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = Random.Range(0,n+1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}