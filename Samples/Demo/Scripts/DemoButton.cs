using BardicBytes.EventVars;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BardicBytes.EventVarsDemo
{
    public class DemoButton : MonoBehaviour
    {
        [field: SerializeField]
        public Button Button { get; private set; }

        [field: SerializeField]
        public TextMeshProUGUI Text { get; private set; }

        [SerializeField]
        private string[] textPool = default;

        [SerializeField]
        private StringEventVar stringEvent = default;

        private bool isQuitting = false;

        private void OnValidate()
        {
            if (Text == null) Text = GetComponentInChildren<TextMeshProUGUI>();
            if(Button == null) Button = GetComponentInChildren<Button>();
        }

        private void OnApplicationQuit()
        {
            isQuitting = true;
        }

        private void OnEnable()
        {
            Button.onClick.AddListener(HandleClick);
            Text.text = textPool[Random.Range(0, textPool.Length)];
        }

        private void OnDisable()
        {
            if (isQuitting) return;
            Button.onClick.RemoveListener(HandleClick);
        }

        private void HandleClick() => stringEvent.Raise(Text.text);
    }
}