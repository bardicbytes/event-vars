// alex@bardicbytes.com

using BardicBytes.EventVars;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This behaviour uses trigger collision to detect rigidbodies with EventVarInstancers.
/// If an instancer is found, and the instancer has the healthEventVar, 
/// the instancer's healthEventVar clone will have it's health raised or lowered over time.
/// </summary>
public class HealthChangeZone : MonoBehaviour
{
    [SerializeField]
    private float damagePerSecond;
    [SerializeField]
    private FloatEventVar healthEventVar;

    private Dictionary<Rigidbody, EventVarInstancer> trackedTargets = new Dictionary<Rigidbody, EventVarInstancer>(1);

    private bool isQuitting = false;

    private void OnApplicationQuit() => isQuitting = true;

    // clear the list of tracked targets when disabled
    private void OnDisable()
    {
        if (isQuitting) return;
        trackedTargets.Clear();
    }

    /// <summary>
    /// begin tracking targets when they enter the tirgger
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody == null) return;
        if (trackedTargets.ContainsKey(other.attachedRigidbody)) return;

        TrackTarget(other);
    }

    /// <summary>
    /// continue tracking targets and catch targets that entered were missed on trigger enter
    /// </summary>
    private void OnTriggerStay(Collider other)
    {
        if (other.attachedRigidbody == null) return;

        if (!trackedTargets.ContainsKey(other.attachedRigidbody)) TrackTarget(other);

        var instancer = trackedTargets[other.attachedRigidbody];
        var instance = instancer.GetInstance(healthEventVar);

        instance.Raise(instance.Value - damagePerSecond * Time.deltaTime);
    }

    /// <summary>
    /// remove targets that leave the trigger
    /// </summary>
    private void OnTriggerExit(Collider other)
    {
        if (other.attachedRigidbody == null) return;

        if (!trackedTargets.ContainsKey(other.attachedRigidbody)) return;
        trackedTargets.Remove(other.attachedRigidbody);
    }

    /// <summary>
    /// if the argument "other" has an attachedRigidbody and that rigidbody has an event var instancer,
    /// track it
    /// </summary>
    private void TrackTarget(Collider other)
    {
        if (other.attachedRigidbody == null) return;

        var otherInstancer = other.attachedRigidbody.GetComponent<EventVarInstancer>();

        if (otherInstancer == null)
        {
            Debug.Log($"Error in {name}. target {other.attachedRigidbody.name} has no instancer");
            return;
        }

        //Debug.Log($"Tracking {otherInstancer}");
        trackedTargets.Add(other.attachedRigidbody, otherInstancer);
    }
}