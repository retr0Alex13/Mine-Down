using System;
using UnityEngine;

public class OnLevelStartTrigger : MonoBehaviour
{
    public static event Action<Vector3> OnLevelStartTriggered;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.TryGetComponent(out PlayerMove playerMove))
        {
            gameObject.SetActive(false);
            OnLevelStartTriggered?.Invoke(transform.position);
        }
    }
}
