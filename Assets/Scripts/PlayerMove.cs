using System.Collections;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField]
    private SwipeDetection swipeDetection;
    [SerializeField]
    private float blockWidth = 1f;
    [SerializeField]
    private float moveSpeed = 15f;

    private Rigidbody body;
    private Vector3 targetPosition;
    private Vector2 swipeDirection;

    private void Start()
    {
        swipeDetection.OnSwipeDirection += MovePlayer;
        body = GetComponent<Rigidbody>();
    }

    private void MovePlayer(Vector2 direction)
    {
        swipeDirection = direction;
        StartCoroutine(MoveToPoint());
    }

    private IEnumerator MoveToPoint()
    {
        SetTargetDirection();

        while (Vector3.Distance(transform.position, targetPosition) > 0f)
        {
            Vector3 newPosition = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            body.MovePosition(newPosition);
            yield return null;
        }
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
