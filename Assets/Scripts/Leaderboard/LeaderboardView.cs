using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardView : MonoBehaviour
{
    [SerializeField] private Button submitScoreButton;
    [SerializeField] private TMP_InputField nameField;

    private void Start()
    {
        submitScoreButton.interactable = true;
    }

    private void Update()
    {
        if (nameField.text.Length > 0)
        {
            submitScoreButton.interactable = true;
        }
        else
        {
            submitScoreButton.interactable = false;
        }
    }

}
