using System.Collections;
using UnityEngine;

public class SwipeDetection : MonoBehaviour
{
    #region events
    public delegate void SwipeDetectionAction(Vector2 direction);
    public event SwipeDetectionAction OnHorizontalSwipe;
    public event SwipeDetectionAction OnVerticalSwipe;
    #endregion

    [SerializeField]
    private float minimumDistance = 0.2f;
    [SerializeField]
    private float maximumTime = 1f;
    [SerializeField, Range(0f, 1f)]
    private float directionThreshold = 0.9f;
    [SerializeField]
    private GameObject trail;

    private InputManager inputManager;

    private Vector2 startPosition;
    private float startTime;

    private Vector2 endPosition;
    private float endTime;

    private Coroutine trailCoroutine;

    private void Awake()
    {
        inputManager = InputManager.Instance;
    }

    private void OnEnable()
    {
        inputManager.OnStartTouch += SwipeStart;
        inputManager.OnEndTouch += SwipeEnd;
    }

    private void OnDisable()
    {
        inputManager.OnStartTouch -= SwipeStart;
        inputManager.OnEndTouch -= SwipeEnd;
    }

    private void SwipeStart(Vector2 position, float time)
    {
        if (GameManager.IsGamePaused)
        {
            return;
        }
        startPosition = position;
        startTime = time;
        trail.SetActive(true);
        trail.transform.position = new Vector3(position.x, position.y, trail.transform.position.z);
        trailCoroutine = StartCoroutine(Trail());
    }

    private IEnumerator Trail()
    {
        while (true) 
        {
            trail.transform.position = new Vector3(inputManager.PrimaryPosition().x, inputManager.PrimaryPosition().y, trail.transform.position.z);
            yield return null;
        }
    }

    private void SwipeEnd(Vector2 position, float time)
    {
        trail.SetActive(false);
        StopCoroutine(trailCoroutine);
        endPosition = position;
        endTime = time;
        DetectSwipe();
    }

    private void DetectSwipe()
    {

        if (Vector3.Distance(startPosition, endPosition) >= minimumDistance &&
            (endTime - startTime) <= maximumTime)
        {
            Debug.DrawLine(startPosition, endPosition, Color.red, 5f);
            Vector3 direction = endPosition - startPosition;
            Vector2 direction2D = new Vector2(direction.x, direction.y).normalized;
            SwipeDirection(direction2D);
        }
    }

    private void SwipeDirection(Vector2 direction)
    {
        if (Vector2.Dot(Vector2.up, direction) > directionThreshold)
        {
            OnVerticalSwipe?.Invoke(Vector2.up);
        }
        else if (Vector2.Dot(Vector2.down, direction) > directionThreshold)
        {
            OnVerticalSwipe?.Invoke(Vector2.down);
        }
        else if (Vector2.Dot(Vector2.left, direction) > directionThreshold)
        {
            OnHorizontalSwipe?.Invoke(Vector2.left);
        }
        else if (Vector2.Dot(Vector2.right, direction) > directionThreshold)
        {
            OnHorizontalSwipe?.Invoke(Vector2.right);
        }
    }
}
