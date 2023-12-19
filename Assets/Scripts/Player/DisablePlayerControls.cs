using UnityEngine;

public class DisablePlayerControls : MonoBehaviour
{
    private PlayerMove playerMove;
    private PlayerAttack playerAttack;

    private void Awake()
    {
        playerMove = GetComponent<PlayerMove>();
        playerAttack = GetComponent<PlayerAttack>();
    }

    private void OnEnable()
    {
        PlayerHealth.OnPlayerDie += DisablePlayerControl;
    }

    private void OnDisable()
    {
        PlayerHealth.OnPlayerDie -= DisablePlayerControl;
    }

    private void DisablePlayerControl()
    {
        playerMove.enabled = false;
        playerAttack.enabled = false;
    }
}
