using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StepByStepLoader : MonoBehaviour
{
    [Serializable]
    public class StepInfo
    {
        public Vector3 _position;
        public GameObject _prefab;
        public string alertText;
    }

    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private CanvasGroup _alertGroup;

    [SerializeField]
    private List<StepInfo> _steps;

    [SerializeField]
    private float _fadeDur = .5f;
    [SerializeField]
    private float _hangDur = 5f;

    private int _stepIndex = -1;
    private Coroutine _alertCoroutine = null;

    private void Awake()
    {
        _text.text = "";
        button.onClick.AddListener(NextStep);
    }

    public void NextStep()
    {
        if (++_stepIndex >= _steps.Count)
        {
            _stepIndex = _steps.Count - 1;
        }
        var step = _steps[_stepIndex];
        Instantiate(step._prefab, step._position, step._prefab.transform.rotation).name = step._prefab.name + "_" + _stepIndex;
        if (_alertCoroutine != null) StopCoroutine(_alertCoroutine);
        _alertCoroutine = StartCoroutine(PulseAlert());
    }

    private IEnumerator PulseAlert()
    {
        _alertGroup.alpha = 0;
        _text.text = _steps[_stepIndex].alertText;
        float start = Time.time;
        float end = start + _fadeDur;
        while(Time.time < end)
        {
            yield return null;
            var t = Mathf.InverseLerp(start, end, Time.time);
            _alertGroup.alpha = t;
        }
        yield return new WaitForSeconds(_hangDur);
        start = Time.time;
        end = start + _fadeDur;
        while (Time.time < end)
        {
            yield return null;
            var t = Mathf.InverseLerp(start, end, Time.time);
            _alertGroup.alpha = 1-t;
        }
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < _steps.Count; ++i)
        {
            Gizmos.DrawWireSphere(_steps[i]._position, .25f);
        }
    }
}
