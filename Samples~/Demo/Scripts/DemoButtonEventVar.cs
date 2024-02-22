//alex@bardicbytes.com

using BardicBytes.EventVars;
using UnityEngine;

namespace BardicBytes.EventVarsDemo
{
    [CreateAssetMenu(menuName = "BardicBytes/Demo/DemoButtonEventVar")]
    public class DemoButtonEventVar : EventVar<DemoButton>
    {
        public override DemoButton GetTypedValue(EventVarInstanceData bc) => bc.UnityObjectValue as DemoButton;

        protected override void SetInstanceConfigValue(DemoButton val, EventVarInstanceData config) => config.UnityObjectValue = val;
    }
}