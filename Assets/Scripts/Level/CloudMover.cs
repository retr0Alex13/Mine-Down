using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMover : MonoBehaviour
{
    [SerializeField] private float speed = 2f; // Adjust the speed as needed
    private float startPosition;

    void Start()
    {
        startPosition = transform.position.x;
    }

    void Update()
    {
        MoveCloud();
    }

    private void MoveCloud()
    {
        float newPosition = Mathf.Repeat(Time.time * speed, Mathf.Abs(startPosition * 2f)) - Mathf.Abs(startPosition);
        transform.position = new Vector3(newPosition, transform.position.y, transform.position.z);
    }
}
