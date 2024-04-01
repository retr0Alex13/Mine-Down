using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPresenter : MonoBehaviour
{
    [SerializeField] private Animator menuAnimator;
    [SerializeField] private PlayerLanding player;

    private Vector3 initialPlayerPosition;

    private void OnEnable()
    {
        player.OnPlayerLanded += GetInitialPlayerPosition;
    }

    private void OnDisable()
    {
        player.OnPlayerLanded -= GetInitialPlayerPosition;
    }

    private void Update()
    {
        if (!player.IsFirstLanded)
        {
            return;
        }
        if (player.transform.position.y < initialPlayerPosition.y - 0.01f)
        {
            menuAnimator.SetBool("IsMenuClosed", true);
        }
    }

    private void GetInitialPlayerPosition()
    {
        initialPlayerPosition = player.GetInitialPlayerPosition();
    }
}
