// alex@bardicbytes.com

using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

namespace BardicBytes.EventVars.BigDemo
{
    public class StatPanel : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _text;
        [SerializeField]
        private float _updateRate = 1;
        [SerializeField]
        private FloatEventVar _statEventVar = default;
        [SerializeField]
        private EventVarInstancerEventVar _onInstancerRegistration = default;

        private List<EventVarInstancer> _instancers;

        private float _lastUpdateTime = 0;

        private bool isQuitting = false;

        private void Awake()
        {
            _instancers = new List<EventVarInstancer>(FindObjectsOfType<EventVarInstancer>());
        }

        private void OnEnable()
        {
            _onInstancerRegistration.AddListener(HandleInstancerRegistration);
        }

        private void OnDisable()
        {
            if (isQuitting) return;
            _onInstancerRegistration.RemoveListener(HandleInstancerRegistration);
        }

        private void OnApplicationQuit()
        {
            isQuitting = true;
        }

        private void Update()
        {
            if (Time.time < _lastUpdateTime + (1 / _updateRate)) return;

            StringBuilder output = new StringBuilder();
            for (int i = 0; i < _instancers.Count; i++)
            {
                EventVarInstancer instancer = _instancers[i];
                if (instancer == null)
                {
                    _instancers.RemoveAt(i);
                    i--;
                    continue;
                }

                if (!instancer.HasInstance(_statEventVar)) continue;
                var instance = instancer.GetInstance(_statEventVar);
                output.AppendLine($"{instancer.name}-{instance.name}: {(Mathf.Round(instance.Value))}");
            }
            _text.text = output.ToString();
        }

        private void HandleInstancerRegistration(EventVarInstancer arg0)
        {
            Debug.Assert(!_instancers.Contains(arg0));

            _instancers.Add(arg0);
        }
    }
}