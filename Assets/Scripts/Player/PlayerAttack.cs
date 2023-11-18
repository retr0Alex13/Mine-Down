using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField]
    private SwipeDetection swipeDetection;

    [SerializeField]
    private float attackRange = 1f;

    private Vector2 attackDirection;
    private PlayerMove playerMove;

    private void Awake()
    {
        playerMove = GetComponent<PlayerMove>();
    }

    private void Start()
    {
        swipeDetection.OnVerticalSwipe += VerticalAttack;
        swipeDetection.OnHorizontalSwipe += HorizontalAttack;
    }

    private void HorizontalAttack(Vector2 direction)
    {
        attackDirection = direction;
        Attack();
    }

    private void VerticalAttack(Vector2 direction)
    {
        if (playerMove.IsMoving)
        {
            return;
        }
        attackDirection = direction;
        Attack();
    }

    private void Attack()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, attackDirection, out hit, attackRange))
        {
            if (hit.transform.TryGetComponent(out IDestroyable destroyable))
            {
                destroyable.Destroy();
            }
        }
    }
}
