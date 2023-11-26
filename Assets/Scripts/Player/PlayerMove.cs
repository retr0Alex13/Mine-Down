using System.Collections;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private SwipeDetection swipeDetection;
    [SerializeField] private float blockWidth = 1f;
    [SerializeField] private float moveSpeed = 15f;
    [SerializeField] private float groundCheckRadius = 0.5f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask playerLayerMask;

    private Rigidbody body;
    private Vector3 targetPosition;
    private Vector2 swipeDirection;

    public bool IsMoving { get; private set; }
    public bool IsGrounded {  get; private set; }

    private void Start()
    {
        swipeDetection.OnHorizontalSwipe += MovePlayer;
        body = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Physics.CheckSphere(groundCheck.position, groundCheckRadius, playerLayerMask))
        {
            IsGrounded = true;
        }
        else
        {
            IsGrounded = false;
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

        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, blockWidth))
        {
            if (hit.transform.TryGetComponent(out Wall wall))
            {
                return;
            }
        }
        swipeDirection = direction;
        SetTargetDirection();
        StartCoroutine(MoveToPoint());
    }

    private IEnumerator MoveToPoint()
    {
        IsMoving = true;

        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            Vector3 newPosition = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            body.MovePosition(newPosition);
            yield return null;
        }

        IsMoving = false;
    }

    private void SetTargetDirection()
    {
        if (swipeDirection == Vector2.left)
        {
            targetPosition = transform.position + new Vector3(-1f, 0f, 0f) * blockWidth;
        }
        else if (swipeDirection == Vector2.right)
        {
            targetPosition = transform.position + new Vector3(1f, 0f, 0f) * blockWidth;
        }
    }
}
