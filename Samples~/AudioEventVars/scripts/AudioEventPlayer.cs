//alex@bardicbytes.com

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BardicBytes.EventVars.AudioEventVar
{
    /// <summary>
    /// This component is responsible for listening to AudioEventVars and managing the spawning and pooling of audio sources.
    /// </summary>
    public class AudioEventPlayer : MonoBehaviour
    {
        private enum CapacityExceededMode
        {
            Ignore = 0,
            RecycleOldest = 1
        }

        [SerializeField]
        protected AudioSource _audioSourceTemplate;

        [SerializeField]
        private int _maxCount = 12;

        [SerializeField]
        private int _initialPoolSize = 3;
        [SerializeField]
        private CapacityExceededMode _capacityMode = CapacityExceededMode.Ignore;

        [SerializeField]
        private List<AudioEventVar> _audioEvents = default;

        private Queue<AudioSource> _pool;
        private List<AudioSource> _playingSources;

        private bool _templateIsPrefab = false;

        private void OnValidate()
        {
            if (_audioSourceTemplate != null && _audioSourceTemplate.gameObject == gameObject)
            {
                Debug.LogError("The AudioSource component must be attached to a game object separate from the AudioEventPlayer, such as a child or prefab.");
            }
        }

        private void Awake()
        {
            if (_audioSourceTemplate == null)
            {
                _audioSourceTemplate = GetComponentInChildren<AudioSource>();
            }

            if (_audioSourceTemplate == null)
            {
                var go = new GameObject("AudioSource_Template");
                go.transform.parent = transform;
                _audioSourceTemplate = go.AddComponent<AudioSource>();
                _audioSourceTemplate.playOnAwake = false;
                _audioSourceTemplate.volume = 0.8f;
            }

            _pool = new Queue<AudioSource>();
            _playingSources = new List<AudioSource>();

            _templateIsPrefab = PrefabUtility.GetPrefabAssetType(_audioSourceTemplate.gameObject) != PrefabAssetType.NotAPrefab;

            if (!_templateIsPrefab)
            {
                _audioSourceTemplate.volume = .8f;
                _audioSourceTemplate.gameObject.SetActive(false);
            }

            for (int i = 0; i < _initialPoolSize; i++)
            {
                AddAudioSourceToPool();
            }

            for (int i = 0; i < _audioEvents.Count; i++)
            {
                if (_audioEvents[i] == null) continue;
                _audioEvents[i].AddListener(Play);
            }
        }

        private void AddAudioSourceToPool()
        {
            var source = Instantiate(_audioSourceTemplate, _templateIsPrefab ? null : _audioSourceTemplate.transform.parent);
            source.gameObject.name = $"AudioSource Pooled - {(_pool.Count + _playingSources.Count)}";
            source.playOnAwake = false;
            source.mute = false;
            source.gameObject.SetActive(false);
            _pool.Enqueue(source);
        }

        private AudioSource SpawnAudioSource()
        {
            if (_playingSources.Count >= _maxCount && _capacityMode == CapacityExceededMode.Ignore) return null;
            if (_playingSources.Count >= _maxCount && _capacityMode == CapacityExceededMode.RecycleOldest)
            {
                _playingSources[0].Stop();
                return _playingSources[0];
            }

            if (_pool.Count <= 0 && _playingSources.Count < _maxCount) AddAudioSourceToPool();

            var s = _pool.Dequeue();
            _playingSources.Add(s);
            s.gameObject.SetActive(true);

            return s;
        }

        private IEnumerator MonitorAudioSource(AudioSource audioSource, AudioEventPlaybackConfig config)
        {
            while (audioSource.isActiveAndEnabled && audioSource.isPlaying)
            {
                yield return null;
                if (config.target != null && config.target.gameObject.activeInHierarchy)
                {
                    audioSource.transform.position = config.target.position;
                }
            }
            audioSource.gameObject.SetActive(false);
            _pool.Enqueue(audioSource);
            _playingSources.Remove(audioSource);
        }

        /// <summary>
        /// Uses the config argument to configure an AudioSource and play it.
        /// </summary>
        /// <param name="config">the information needed to configure an AudioSource</param>
        /// <returns></returns>
        public void Play(AudioEventPlaybackConfig config)
        {
            var audioSource = SpawnAudioSource();

            if (audioSource == null) return;

            config.ApplyTo(audioSource);
            audioSource.Play();

            StartCoroutine(MonitorAudioSource(audioSource, config));
            return;
        }
    }
}