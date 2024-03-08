using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObjects : MonoBehaviour
{
    [SerializeField] private GameObject[] objectsToDestroy;

    public void DestroyObject()
    {
        if (objectsToDestroy.Length == 0)
        {
            return;
        }
        for (int i = 0; i < objectsToDestroy.Length; i++)
        {
            Destroy(objectsToDestroy[i]);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            DestroyObject();
        }
    }
}
