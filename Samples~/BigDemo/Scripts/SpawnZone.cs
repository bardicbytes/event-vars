// alex@bardicbytes.com

using System;
using System.Collections.Generic;
using UnityEngine;

namespace BardicBytes.EventVars.BigDemo
{

    public class SpawnZone : MonoBehaviour
    {
        [Serializable]
        private struct TrackedTargetInfo
        {
            public EventVarInstancer _instancer;
            public float _trackStartTime;
        }

        [SerializeField]
        private float _spawnInterval;
        [SerializeField]
        private GameObject _spawnPrefab;

        private Dictionary<Rigidbody, TrackedTargetInfo> _trackedTargets = new Dictionary<Rigidbody, TrackedTargetInfo>(1);

        private float _lastSpawnTime = 0;
        private bool _isQuitting = false;

        private void OnApplicationQuit() => _isQuitting = true;

        private void OnDisable()
        {
            if (_isQuitting) return;
            _trackedTargets.Clear();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.attachedRigidbody == null) return;
            if (_trackedTargets.ContainsKey(other.attachedRigidbody)) return;

            TrackTarget(other);
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.attachedRigidbody == null) return;

            if (!_trackedTargets.ContainsKey(other.attachedRigidbody)) TrackTarget(other);

            if (Time.time - _lastSpawnTime >= _spawnInterval && _trackedTargets[other.attachedRigidbody]._trackStartTime < Time.time - _spawnInterval)
            {
                _lastSpawnTime = Time.time;
                Instantiate(_spawnPrefab);
            }
        }

        private void TrackTarget(Collider other)
        {
            if (other.attachedRigidbody == null) return;

            var otherInstancer = other.attachedRigidbody.GetComponent<EventVarInstancer>();

            if (otherInstancer == null)
            {
                Debug.Log($"Error in {name}. target {other.attachedRigidbody.name} has no instancer");
                return;
            }

            _trackedTargets.Add(other.attachedRigidbody, new TrackedTargetInfo()
            {
                _instancer = otherInstancer,
                _trackStartTime = Time.time
            });
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.attachedRigidbody == null) return;

            if (!_trackedTargets.ContainsKey(other.attachedRigidbody)) return;
            _trackedTargets.Remove(other.attachedRigidbody);
        }

    }
}