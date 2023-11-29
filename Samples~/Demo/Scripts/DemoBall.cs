// alex@bardicbytes.com
using BardicBytes.EventVars;
using UnityEngine;

namespace BardicBytes.EventVarsDemo
{
    public class DemoBall : MonoBehaviour
    {
        [SerializeField] private Rigidbody rigidBody;
        [field: SerializeField] public FloatEventVar.Field Speed { get; private set; }

        private void OnValidate()
        {
            Speed.Validate();
        }

        public void DoRandomPush()
        {
            var dir = new Vector3(2 * Random.value - 1, Random.value, 2 * Random.value - 1);
            rigidBody.velocity += dir.normalized * Speed;
        }
    }
}