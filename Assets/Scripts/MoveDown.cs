using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDown : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 4f;
    [SerializeField] private float speedMultiplier = 0.1f;
    private float speed = 1f;
    private Rigidbody rigidBody;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        OnLevelEndTrigger.OnLevelEndTriggered += SetSpeed;
    }

    private void OnDisable()
    {
        OnLevelEndTrigger.OnLevelEndTriggered -= SetSpeed;
    }

    private void SetSpeed()
    {
        speed += speedMultiplier;
        if (speed > maxSpeed)
        {
            speed = maxSpeed;
        }
    }

    private void FixedUpdate()
    {
        rigidBody.MovePosition(transform.position + Vector3.down * Time.deltaTime * speed);
    }
}
