using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MenuPresenter : MonoBehaviour
    {
        // Visualize restart delay
        [SerializeField] private MenuView menuView;
        [SerializeField] private PlayerLanding player;

        [SerializeField] private Button pauseButton;
        [SerializeField] private Button continueButton;
        [SerializeField] private Button soundButton;

        [SerializeField] private GameObject tutorialSprites;

        private PlayerModel playerModel;
        private const float initialPositionTolerance = 0.1f;
        private bool shouldCheckMenuAndHudVisibility = true;


        public MenuView GetMenuView()
        {
            return menuView;
        }

        private void Awake()
        {
            playerModel = new PlayerModel();

            player.OnPlayerLanded += HandlePlayerLanded;
        }

        private void OnDestroy()
        {
            player.OnPlayerLanded -= HandlePlayerLanded;

            pauseButton.onClick.RemoveAllListeners();
            continueButton.onClick.RemoveAllListeners();
            soundButton.onClick.RemoveAllListeners();
        }

        private void Start()
        {
            pauseButton.onClick.AddListener(GameManager.Instance.PauseGame);
            continueButton.onClick.AddListener(GameManager.Instance.ResumeGame);
            soundButton.onClick.AddListener(GameManager.Instance.ToggleSound);
        }

        private void Update()
        {
            if (shouldCheckMenuAndHudVisibility && playerModel.IsFirstLanded)
                CheckPlayerPosition();
        }

        private void CheckPlayerPosition()
        {
            float positionDifference = playerModel.GetPositionDifference(player.transform.position);
            if (positionDifference > initialPositionTolerance)
            {
                menuView.SetMenuVisibility(true);
                menuView.SetHudVisibility(true);
                tutorialSprites.SetActive(false);

                shouldCheckMenuAndHudVisibility = false;
            }
        }

        public void ToggleMenuAndHud()
        {
            shouldCheckMenuAndHudVisibility = false;

            bool menuVisibility = !menuView.GetMenuVisibility();
            bool hudVisibility = !menuView.GetHudVisibility();

            menuView.SetMenuVisibility(menuVisibility);
            menuView.SetHudVisibility(hudVisibility);
        }

        private void HandlePlayerLanded()
        {
            playerModel.SetInitialPosition(player.GetInitialPlayerPosition());
            shouldCheckMenuAndHudVisibility = true;
        }
    }
}