using System;
using UnityEngine;

public class OnLevelEndTrigger : MonoBehaviour
{
    public static event Action OnLevelEndTriggered;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.TryGetComponent(out PlayerMove playerMove))
        {
            gameObject.SetActive(false);
            OnLevelEndTriggered?.Invoke();
        }
    }
}
