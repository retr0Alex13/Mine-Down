using UnityEngine;

public class MoveDown : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    [SerializeField] private float maxSpeed = 4f;
    [SerializeField] private float speedMultiplier = 0.1f;
    private bool moving = true;
    private Rigidbody rigidBody;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        OnLevelEndTrigger.OnLevelEndTriggered += UpdateSpeed;
        OnLevelEndTrigger.OnLevelEndTriggered += Stop;

        OnLevelStartTrigger.OnLevelStartTriggered += Resume;
    }

    private void OnDisable()
    {
        OnLevelEndTrigger.OnLevelEndTriggered -= UpdateSpeed;
        OnLevelEndTrigger.OnLevelEndTriggered -= Stop;

        OnLevelStartTrigger.OnLevelStartTriggered -= Resume;
    }

    private void Stop()
    {
        ToggleMoving(false);
    }

    private void Resume(Vector3 position)
    {
        ToggleMoving(true);
    }

    private void ToggleMoving(bool movement)
    {
        moving = movement;
    }

    private void UpdateSpeed()
    {
        speed += speedMultiplier;
        if (speed > maxSpeed)
        {
            speed = maxSpeed;
        }
    }

    private void FixedUpdate()
    {
        if (!moving)
        {
            return;
        }
        rigidBody.MovePosition(transform.position + Vector3.down * Time.deltaTime * speed);
    }
}
