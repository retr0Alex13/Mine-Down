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
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private float landingDelay = 0.2f; // New variable for landing delay

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
    public bool IsGrounded { get; private set; }
    private bool isLanding; // New variable to track landing delay

    public bool IsLanding => isLanding; // Public property to expose isLanding state

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

    private void FixedUpdate()
    {
        UpdateIsGrounded();
    }

    private void UpdateIsGrounded()
    {
        bool wasGrounded = IsGrounded;
        IsGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayerMask);
        animator.SetBool("IsGrounded", IsGrounded);

        if (IsGrounded && !wasGrounded)
        {
            StartCoroutine(LandingDelay()); // Start landing delay when player lands
        }
    }

    private IEnumerator LandingDelay()
    {
        isLanding = true;
        yield return new WaitForSeconds(landingDelay);
        isLanding = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(groundCheck.position, groundCheckRadius);
    }

    private void Update()
    {
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
        if (playerModelTransform.position.y < target.position.y)
        {
            Vector3 directionToTarget = target.position - playerModelTransform.position;
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget, Vector3.up);
            playerModelTransform.rotation = Quaternion.Lerp(playerModelTransform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void MovePlayer(Vector2 direction)
    {
        if (IsMoving || !IsGrounded || isLanding)
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
            if (hit.transform.GetComponent<Wall>() != null)
            {
                return true;
            }
            Block block = hit.transform.GetComponent<Block>();
            if (block != null && block.HealthPoints > 0)
            {
                return true;
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
            yield return new WaitForFixedUpdate();
        }

        IsMoving = false;
        animator.SetBool("IsRunning", IsMoving);

        // Re-check grounded status after moving
        yield return new WaitForSeconds(0.1f); // Small delay to ensure grounded status is updated
        UpdateIsGrounded();
    }

    private void SetTargetDirection()
    {
        if (swipeDirection == Vector2.left)
        {
            targetPosition = transform.position + new Vector3(-1f, 0f, 0f) * blockWidth;
            rotationTarget = leftRotationTarget;
        }
        else if (swipeDirection == Vector2.right)
        {
            targetPosition = transform.position + new Vector3(1f, 0f, 0f) * blockWidth;
            rotationTarget = rightRotationTarget;
        }
    }
}
