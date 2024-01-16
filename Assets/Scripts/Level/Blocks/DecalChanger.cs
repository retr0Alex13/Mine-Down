using System.Collections.Generic;
using UnityEngine;

public class DecalChanger : MonoBehaviour
{
    private int nextGameObjectIndex = 0;
    private List<GameObject> decals;

    private Block block;

    private void OnEnable()
    {
        block.OnBlockDamaged += ChangeDecal;
    }

    private void OnDisable()
    {
        block.OnBlockDamaged -= ChangeDecal;
    }

    private void Awake()
    {
        block = GetComponent<Block>();
    }

    private void Start()
    {
        decals = new List<GameObject>();
        foreach (Transform child in transform)
        {
            decals.Add(child.gameObject);
        }
    }

    private void ChangeDecal()
    {
        if (decals == null)
        {
            return;
        }
        if (decals.Count > 0)
        {
            decals[nextGameObjectIndex].SetActive(true);
            nextGameObjectIndex = (nextGameObjectIndex + 1) % decals.Count;
        }
    }
}
