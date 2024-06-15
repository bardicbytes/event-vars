// alex@bardicbytes.com

using BardicBytes.EventVars;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class StatPanel : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _text;
    [SerializeField]
    private float _updateRate = 1;
    [SerializeField]
    private FloatEventVar _statEventVar = default;

    private List<EventVarInstancer> _instancers = null;

    private float _lastUpdateTime = 0;

    private void OnEnable()
    {
        _instancers = new List<EventVarInstancer>(FindObjectsOfType<EventVarInstancer>());
    }

    private void Update()
    {
        if (Time.time < _lastUpdateTime + (1 / _updateRate)) return;

        StringBuilder output = new StringBuilder();
        foreach(EventVarInstancer instancer in _instancers)
        {
            if (!instancer.HasInstance(_statEventVar)) continue;
            var instance = instancer.GetInstance(_statEventVar);
            output.AppendLine($"{instancer.name}-{instance.name}: {instance.Value}");
        }
        _text.text = output.ToString();
    }
}
