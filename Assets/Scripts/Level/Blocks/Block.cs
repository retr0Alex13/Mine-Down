using System;
using UnityEngine;

public class Block : MonoBehaviour, IDestroyable
{
    public static event Action<int> OnOreCollected;

    [SerializeField] private int pointsAmount = 50;
    [SerializeField] private int healthPoints = 1;

    private ParticleInstantiator particleEmitter;

    private void Awake()
    {
        particleEmitter = GetComponent<ParticleInstantiator>();
    }

    public void Damage(int damage, Vector2 attackDirection)
    {
        healthPoints -= damage;

        if (healthPoints <= 0)
        {
            Destroy(gameObject);
            healthPoints = 0;

            OnOreCollected?.Invoke(pointsAmount);

            if (attackDirection == null  || particleEmitter == null)
            {
                return;
            }
            particleEmitter.InstantiateParticleInDirection(attackDirection);
        }
    }
}
