//alex@bardicbytes.com

using UnityEngine;

namespace BardicBytes.EventVars
{
    public class GameObjectEventVarInitializer : EventVarInitializer<GameObjectEventVar, GameObject>
    {
        protected override void RaiseEventVar() => _target.Raise(_initialValue);
    }
}
