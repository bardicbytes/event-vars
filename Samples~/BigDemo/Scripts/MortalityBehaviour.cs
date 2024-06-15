// alex@bardicbytes.com

using BardicBytes.EventVars;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(EventVarInstancer))]
public class MortalityBehaviour : MonoBehaviour
{
    [SerializeField] private EventVarInstancer _instancer;
    [SerializeField] private FloatEventVar _healthEventVar;

    [field: SerializeField] public UnityEvent<MortalityBehaviour> OnMinHealth { get; private set; } = default;

    private FloatEventVar _healthInstance;
    private void OnEnable()
    {
        _healthInstance = _instancer.GetInstance(_healthEventVar);
        _healthInstance.AddListener(HandleHealthChanged);
    }
    private void OnDisable()
    {
        _healthInstance.RemoveListener(HandleHealthChanged);
        _healthInstance = null;
    }

    private void HandleHealthChanged(float newHealth)
    {
        if(Mathf.Approximately(newHealth, _healthInstance.MinValue))
        {
            // raising without an argument will reset the value to its initial
            _healthInstance.Raise();
            OnMinHealth.Invoke(this);
        }
    }

}
