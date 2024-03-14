using UnityEngine;

public class OnTriggerDestroy : MonoBehaviour
{
    [SerializeField] private int damageAmount = 1;
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.TryGetComponent(out Block block))
        {
            block.SetDamagedByPlayer(false);
        }
        if (other.transform.TryGetComponent(out IDestroyable destroyable))
        {
            destroyable.Damage(damageAmount, Vector2.down);
        }
    }
}
