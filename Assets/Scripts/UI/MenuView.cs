using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MenuView : MonoBehaviour
    {
        [SerializeField] private Animator menuAnimator;
        [SerializeField] private Animator hudAnimator;
        [SerializeField] private Button pauseButton;
        [SerializeField] private Button continueButton;

        [SerializeField] private MenuPresenter menuPresenter;

        private void Start()
        {
            pauseButton.onClick.AddListener(menuPresenter.ToggleMenuAndHud);
        }

        public void SetMenuVisibility(bool isVisible)
        {
            menuAnimator.SetBool("IsMenuClosed", isVisible);
        }

        public void SetHudVisibility(bool isVisible)
        {
            hudAnimator.SetBool("IsHudOpen", isVisible);
        }

        public void SetContinueButtonVisibility(bool isVisible)
        {
            continueButton.gameObject.SetActive(isVisible);
        }

        public bool GetMenuVisibility()
        {
            return menuAnimator.GetBool("IsMenuClosed");
        }

        public bool GetHudVisibility()
        {
            return hudAnimator.GetBool("IsHudOpen");
        }
    }
}