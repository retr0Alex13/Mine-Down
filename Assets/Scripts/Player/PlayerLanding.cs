using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLanding : MonoBehaviour
{
    PlayerMove playerMove;

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
        }
    }
}
