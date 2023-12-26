using UnityEngine;

public class SpikeBlock : MonoBehaviour
{
    [SerializeField] private GameObject spike;

    private void OnDestroy()
    {
        Destroy(spike);
    }
}
