// alex@bardicbytes.com

using BardicBytes.EventVars;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    [SerializeField]
    private float damagePerSecond;
    [SerializeField]
    private FloatEventVar healthEventVar;

    private Dictionary<Rigidbody, EventVarInstancer> trackedTargets = new Dictionary<Rigidbody, EventVarInstancer>(1);

    private bool isQuitting = false;

    private void OnApplicationQuit() => isQuitting = true;

    private void OnDisable()
    {
        if (isQuitting) return;
        trackedTargets.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody == null) return;
        if (trackedTargets.ContainsKey(other.attachedRigidbody)) return;

        TrackTarget(other);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.attachedRigidbody == null) return;
        if (!trackedTargets.ContainsKey(other.attachedRigidbody)) TrackTarget(other);

        var instancer = trackedTargets[other.attachedRigidbody];
        var instance = instancer.GetInstance(healthEventVar);

        instance.Raise(instance.Value - damagePerSecond * Time.deltaTime);
    }

    private void TrackTarget(Collider other)
    {
        var otherInstancer = other.attachedRigidbody.GetComponent<EventVarInstancer>();
        Debug.Log($"Tracking {otherInstancer}");
        trackedTargets.Add(other.attachedRigidbody, otherInstancer);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!trackedTargets.ContainsKey(other.attachedRigidbody)) return;
        trackedTargets.Remove(other.attachedRigidbody);
    }

}
