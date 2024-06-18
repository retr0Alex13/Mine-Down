using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField]
    private SwipeDetection swipeDetection;

    [SerializeField]
    private int attackAmount = 1;

    [SerializeField]
    private float attackRange = 1f;

    [SerializeField]
    private Animator playerAnimator;

    private UpgradeType upgradeSlot1;
    private UpgradeType upgradeSlot2;

    private Vector2 attackDirection;
    private PlayerMove playerMove;

    private void Awake()
    {
        playerMove = GetComponent<PlayerMove>();
    }

    private void OnEnable()
    {
        swipeDetection.OnVerticalSwipe += VerticalAttack;
        swipeDetection.OnHorizontalSwipe += HorizontalAttack;
    }

    private void OnDisable()
    {
        swipeDetection.OnVerticalSwipe -= VerticalAttack;
        swipeDetection.OnHorizontalSwipe -= HorizontalAttack;
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

    public void Attack()
    {
        RaycastHit[] hits;
        hits = Physics.RaycastAll(transform.position, attackDirection, attackRange);
        foreach (var hit in hits)
        {
            if (hit.transform.TryGetComponent(out Block block))
            {
                block.SetDamagedByPlayer(true);
            }

            if (hit.transform.TryGetComponent(out IDestroyable destroyable))
            {
                destroyable.Damage(attackAmount, attackDirection);

                SoundManager.instance.Play("PickAxe_Mining", true);
            }
        }
    }

    public void SetUpgradeSlot(UpgradeType upgradeType)
    {
        switch (upgradeType)
        {
            case UpgradeType.Strength:
                upgradeSlot1 = upgradeType;
                attackAmount = 2;
                break;
            case UpgradeType.Range:
                upgradeSlot2 = upgradeType;
                attackRange = 1.5f;
                break;
        }
    }
}
