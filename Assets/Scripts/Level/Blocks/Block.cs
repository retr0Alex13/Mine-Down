using System;
using UnityEngine;

public class Block : MonoBehaviour, IDestroyable
{
    public event Action OnBlockDamaged;

    public bool DamagedByPlayer { get; private set; }
    [field: SerializeField] public int HealthPoints { get; private set; }

    public static event Action<int> OnOreCollected;

    [SerializeField] private int pointsAmount = 50;

    private ParticleInstantiator particleEmitter;

    private void Awake()
    {
        particleEmitter = GetComponent<ParticleInstantiator>();
    }

    public void SetDamagedByPlayer(bool variable)
    {
        DamagedByPlayer = variable;
    }

    public void Damage(int damage, Vector2 attackDirection)
    {
        HealthPoints -= damage;

        if (HealthPoints <= 0)
        {
            Destroy(gameObject);
            HealthPoints = 0;

            if (attackDirection == null || particleEmitter == null)
            {
                return;
            }
            particleEmitter.InstantiateParticleInDirection(attackDirection);

            if (!DamagedByPlayer)
            {
                return;
            }
            OnOreCollected?.Invoke(pointsAmount);
        }
        OnBlockDamaged?.Invoke();
    }
}
