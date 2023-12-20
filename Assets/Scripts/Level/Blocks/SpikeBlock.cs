using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeBlock : MonoBehaviour
{
    [SerializeField] private GameObject spike;

    private void OnDestroy()
    {
        Destroy(spike);
    }
}
