//alex@bardicbytes.com

using UnityEngine;

namespace BardicBytes.EventVars.Samples
{ 
    /// <summary>
    /// By configuring the initial value of asset instances of AudioEvent,
    /// those files can be used to trigger sound effects directly.
    /// Alternatively, unique AudioEventData can be passed to the audio event
    /// through the Raise(AudioEventData) method
    /// </summary>
    [CreateAssetMenu(menuName = "BardicBytes/EventVars/Rigidbody EventVar")]
    public class RigidbodyEventVar : EventVar<Rigidbody>
    {
        public override Rigidbody GetTypedValue(EventVarInstanceData data) => data.UnityObjectValue as RigidbodyEventVar;
    }
}