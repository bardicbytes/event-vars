//alex@bardicbytes.com

using BardicBytes.EventVars;
using TMPro;
using UnityEngine;

public class PositionHandler : MonoBehaviour
{
    [SerializeField]
    private Vector3EventVar positionEvent;

    [SerializeField]
    private TextMeshProUGUI outputText;

    private void OnEnable()
    {
        positionEvent.AddListener(HandlePositionChanged);
    }
    
    private void OnDisable()
    {
        positionEvent.RemoveListener(HandlePositionChanged);
    }

    private void HandlePositionChanged(Vector3 arg0)
    {
        outputText.text = arg0.ToString();
        Debug.DrawRay(arg0, Vector3.up, Color.magenta/2, 5);
    }
}
