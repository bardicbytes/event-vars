﻿//alex@bardicbytes.com

using UnityEngine;

namespace BardicBytes.EventVars
{
    public class GameObjectEventVarListener : GenericEventVarListener<GameObject>
    {
        protected override void HandleTypedEventRaised(GameObject data) => base.HandleTypedEventRaised(data);
    }
}