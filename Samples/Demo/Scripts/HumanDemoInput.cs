//alex@bardicbytes.com

using BardicBytes.EventVars;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BardicBytes.EventVarsDemo
{
    public class HumanDemoInput : MonoBehaviour
    {
        [SerializeField]
        private Vector3EventVar worldPositionInput = default;
        [field:Space]
        [field: SerializeField]
        public Camera Camera { get; set; }
        [SerializeField]
        private LayerMask raycastMask = default;

        void Update()
        {
            if(EventSystem.current.IsPointerOverGameObject()) return;

            Vector3 screenPos = Vector3.zero;
            bool didInput = false;

            if (Input.mousePresent && Input.GetKey(KeyCode.Mouse0))
            {
                didInput = true;
                screenPos = Input.mousePosition;
            }

            if(Input.touchSupported && Input.touchCount > 0)
            {
                didInput = true;
                screenPos = Input.touches[0].position;
            }

            if (!didInput) return;

            var ray = Camera.ScreenPointToRay(screenPos);

            RaycastHit hit = default;
            if (Physics.Raycast(ray, out hit, 500, raycastMask, QueryTriggerInteraction.Ignore))
            {
                worldPositionInput.Raise(hit.point);
            }
        }
    }
}