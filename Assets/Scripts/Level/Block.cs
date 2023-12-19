using UnityEngine;

public class Block : MonoBehaviour, IDestroyable
{
    [SerializeField] private int healthPoints = 1;
    public void Damage(int damage)
    {
        healthPoints -= damage;

        if (healthPoints <= 0)
        {
            Destroy(gameObject);
            healthPoints = 0;
        }

    }
}
