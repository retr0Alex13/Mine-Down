using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocksParticle : MonoBehaviour
{
    [SerializeField] private int damageAmount = 1;
    private void OnParticleCollision(GameObject other)
    {
        if (other.transform.TryGetComponent(out PlayerHealth playerHealth))
        {
            playerHealth.Damage(damageAmount, Vector2.down);
        }
    }
}
