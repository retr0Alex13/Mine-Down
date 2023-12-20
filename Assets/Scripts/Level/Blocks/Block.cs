using System;
using UnityEngine;

public class Block : MonoBehaviour, IDestroyable
{
    public static event Action<int> OnOreCollected;

    [SerializeField] private int pointsAmount = 50;
    [SerializeField] private int healthPoints = 1;

    public void Damage(int damage)
    {
        healthPoints -= damage;

        if (healthPoints <= 0)
        {
            Destroy(gameObject);
            healthPoints = 0;
            OnOreCollected?.Invoke(pointsAmount);
        }

    }
}
