using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDestroyable
{
    public static event Action OnPlayerDie;

    [SerializeField] private int healthPoints = 1;
    public void Damage(int damage)
    {
        if (healthPoints <= 0)
        {
            return;
        }

        healthPoints -= damage;

        if (healthPoints <= 0)
        {
            healthPoints = 0;
            OnPlayerDie?.Invoke();
        }
    }
}
