using System.Collections;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private SwipeDetection swipeDetection;
    [SerializeField] private float blockWidth = 1f;
    [SerializeField] private float moveSpeed = 15f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float groundCheckRadius = 0.1f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask playerLayerMask;

    [Space(10)]
    [SerializeField] private Transform playerModelTransform;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform leftRotationTarget;
    [SerializeField] private Transform rightRotationTarget;
    [SerializeField] private Transform defaultRotationTarget;


    private Rigidbody body;
    private Vector3 targetPosition;
    private Transform rotationTarget;
    private Vector2 swipeDirection;

    public bool IsMoving { get; private set; }
    public bool IsGrounded {  get; private set; }

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
    }
    
    private void OnEnable()
    {
        swipeDetection.OnHorizontalSwipe += MovePlayer;
    }

    private void OnDisable()
    {
        swipeDetection.OnHorizontalSwipe -= MovePlayer;
    }

    private void Update()
    {
        if (Physics.CheckSphere(groundCheck.position, groundCheckRadius, playerLayerMask))
        {
            IsGrounded = true;
            animator.SetBool("IsGrounded", IsGrounded);
        }
        else
        {
            IsGrounded = false;
            animator.SetBool("IsGrounded", IsGrounded);
        }

        if (IsMoving)
        {
            RotatePlayerModel(rotationTarget);
        }
        else
        {
            RotatePlayerModel(defaultRotationTarget);
        }
    }

    private void RotatePlayerModel(Transform target)
    {
        // Перевірка, чи гравець досягнув заданої точки
        if (playerModelTransform.position.y < target.position.y)
        {
            // Обчислення вектору, який вказує в напрямку заданої точки
            Vector3 directionToTarget = target.position - playerModelTransform.position;

            // Обчислення обертання для повернення гравця у задану точку
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget, Vector3.up);

            // Застосування обертання до модельки гравця
            playerModelTransform.rotation = Quaternion.Lerp(playerModelTransform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(groundCheck.position, groundCheckRadius);
    }

    private void MovePlayer(Vector2 direction)
    {
        if (IsMoving)
        {
            return;
        }

        if (!IsGrounded)
        {
            return;
        }

        if (IsPathBlocked(direction))
        {
            return;
        }

        swipeDirection = direction;
        SetTargetDirection();
        StartCoroutine(MoveToPoint());
        SoundManager.instance.Play("PlayerMove", false);
    }

    private bool IsPathBlocked(Vector2 checkDirection)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, checkDirection, out hit, blockWidth))
        {
            if (hit.transform.TryGetComponent(out Wall wall))
            {
                return true;
            }
            if (hit.transform.TryGetComponent(out Block block))
            {
                if (block.HealthPoints > 0)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private IEnumerator MoveToPoint()
    {
        IsMoving = true;
        animator.SetBool("IsRunning", IsMoving);

        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            Vector3 newPosition = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            body.MovePosition(newPosition);
            yield return null;
        }

        IsMoving = false;
        animator.SetBool("IsRunning", IsMoving);
    }

    private void SetTargetDirection()
    {
        if (swipeDirection == Vector2.left)
        {
            targetPosition = transform.position + new Vector3(-1f, 0f, 0f) * blockWidth;
            rotationTarget = rightRotationTarget;
        }
        else if (swipeDirection == Vector2.right)
        {
            targetPosition = transform.position + new Vector3(1f, 0f, 0f) * blockWidth;
            rotationTarget = leftRotationTarget;
        }
    }

}
