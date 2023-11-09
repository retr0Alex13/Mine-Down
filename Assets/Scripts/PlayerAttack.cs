using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField]
    private SwipeDetection swipeDetection;

    [SerializeField]
    private float attackRange = 1f;

    private void Start()
    {
        swipeDetection.OnSwipeDirection += Attack;
    }

    private void Update()
    {
        Debug.DrawRay(transform.position, Vector2.down * attackRange, Color.red);
        Debug.DrawRay(transform.position, Vector2.up * attackRange, Color.red);
        Debug.DrawRay(transform.position, Vector2.left * attackRange, Color.red);
        Debug.DrawRay(transform.position, Vector2.right * attackRange, Color.red);
    }

    private void Attack(Vector2 direction)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, attackRange))
        {
            if (hit.transform.TryGetComponent(out IDestroyable destroyable))
            {
                destroyable.Destroy();
            }
        }
    }
}
