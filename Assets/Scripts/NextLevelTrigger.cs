using System;
using UnityEngine;

public class NextLevelTrigger : MonoBehaviour
{
    public static event Action OnNextLevelTriggered;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.TryGetComponent(out PlayerMove playerMove))
        {
            gameObject.SetActive(false);
            OnNextLevelTriggered?.Invoke();
        }
    }
}
