using UnityEngine;

/// <summary>
/// MatchTransform sets its transform's position and rotation to match the target on Update.
/// </summary>
public class MatchTransform : MonoBehaviour
{
    public Transform _target;

    private void Update()
    {
        if (_target == null) return;
        transform.SetPositionAndRotation(_target.position, _target.rotation);
    }

    public void SetTarget(Transform newTarget)
    {
        this._target = newTarget;
    }
}
