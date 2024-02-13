//alex@bardicbytes.com

using UnityEngine;

namespace BardicBytes.EventVarsDemo
{
    public class DemoButtonDeleter : MonoBehaviour
    {
        public void DeleteDemoButton(DemoButton db) => Destroy(db.gameObject);
    }
}