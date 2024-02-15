using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    [SerializeField] private Animator playerAnimator;

    private Rigidbody playerRigidbody;

    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        PlayerHealth.OnPlayerDie += ProcessPlayerDeath;
    }
    private void OnDisable()
    {
        PlayerHealth.OnPlayerDie -= ProcessPlayerDeath;
    }

    private void ProcessPlayerDeath()
    {
        playerRigidbody.AddForce(Vector3.up * 10, ForceMode.Impulse);
        playerRigidbody.AddForce(Vector3.back * 5, ForceMode.Impulse);

        playerAnimator.SetTrigger("IsDead");
    }
}
