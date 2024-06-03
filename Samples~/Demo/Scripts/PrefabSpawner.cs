//alex@bardicbytes.com

using UnityEngine;

namespace BardicBytes.EventVarsDemo
{
    public class PrefabSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject[] prefabs = default;
        [SerializeField, Min(1)] private int loops = 1;
        [SerializeField] private Transform parent = default;
        [SerializeField] private bool spawnOnEnable = true;

        private void OnEnable()
        {
            if (spawnOnEnable) Spawn();
        }

        public void Spawn()
        {
            for(int l = 0; l < loops; l++)
            {
                for(int i = 0; i < prefabs.Length; i++)
                {
                    Instantiate(prefabs[i], parent).SetActive(true);
                }
            }
        }
    }
}