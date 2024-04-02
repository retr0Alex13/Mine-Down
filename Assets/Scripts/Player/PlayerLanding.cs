using System;
using UnityEngine;

public class PlayerLanding : MonoBehaviour
{
    public event Action OnPlayerLanded;

    private PlayerMove playerMove;
    private Vector3 initialPlayerPosition;

    private bool isFirstLanded;
    public bool IsFirstLanded => isFirstLanded;

    private void Awake()
    {
        playerMove = GetComponent<PlayerMove>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (playerMove.IsMoving)
        {
            return;
        }
        if (collision.transform.TryGetComponent(out Block block) || collision.transform.TryGetComponent(out Wall wall))
        {
            SoundManager.instance.Play("PlayerLand", false);
            if (isFirstLanded)
            {
                return;
            }
            initialPlayerPosition = transform.position;
            isFirstLanded = true;
            OnPlayerLanded?.Invoke();
        }
    }

    public Vector3 GetInitialPlayerPosition()
    {
        return initialPlayerPosition;
    }
}
