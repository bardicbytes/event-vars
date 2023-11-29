using BardicBytes.EventVars;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoSpawner : MonoBehaviour
{
    [SerializeField]
    private EventVar spawnEvent;

    [SerializeField]
    private GameObject prefab;

    private void OnEnable()
    {
        spawnEvent.AddListener(SpawnOne);
    }

    private void OnDisable()
    {
        spawnEvent.RemoveListener(SpawnOne);
    }

    private void SpawnOne()
    {
        Instantiate(prefab);
    }
}
