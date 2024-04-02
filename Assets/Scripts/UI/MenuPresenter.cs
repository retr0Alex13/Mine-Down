using UnityEngine;
using UnityEngine.UI;

public class MenuPresenter : MonoBehaviour
{
    [SerializeField] private Animator menuAnimator;
    [SerializeField] private Animator hudAnimator;
    [SerializeField] private Button pauseButton;
    [SerializeField] private PlayerLanding player;

    private Vector3 initialPlayerPosition;
    private bool shouldCheckMenuAndHudVisibility = true;
    private const float initialPositionTolerance = 0.1f;

    private void Awake()
    {
        player.OnPlayerLanded += GetInitialPlayerPosition;
        pauseButton.onClick.AddListener(ToggleMenuAndHud);
    }

    private void OnDestroy()
    {
        player.OnPlayerLanded -= GetInitialPlayerPosition;
    }

    private void Update()
    {
        if (shouldCheckMenuAndHudVisibility)
            CheckMenuAndHudVisibility();
    }

    private void CheckMenuAndHudVisibility()
    {
        if (!player.IsFirstLanded)
            return;

        float positionDifference = Mathf.Abs(player.transform.position.y - initialPlayerPosition.y);
        if (positionDifference > initialPositionTolerance)
        {
            Debug.Log(player.transform.position);
            SetMenuAndHudVisibility(true, true);
            shouldCheckMenuAndHudVisibility = false;
        }
    }

    public void ToggleMenuAndHud()
    {
        bool menuVisibility = !menuAnimator.GetBool("IsMenuClosed");
        bool hudVisibility = !hudAnimator.GetBool("IsHudOpen");
        SetMenuAndHudVisibility(menuVisibility, hudVisibility);
        shouldCheckMenuAndHudVisibility = false;
    }

    private void SetMenuAndHudVisibility(bool menuVisibility, bool hudVisibility)
    {
        menuAnimator.SetBool("IsMenuClosed", menuVisibility);
        hudAnimator.SetBool("IsHudOpen", hudVisibility);
    }

    private void GetInitialPlayerPosition()
    {
        initialPlayerPosition = player.GetInitialPlayerPosition();
        shouldCheckMenuAndHudVisibility = true;
    }
}