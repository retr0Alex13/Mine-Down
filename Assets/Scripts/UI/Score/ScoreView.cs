using TMPro;
using UnityEngine;

public class ScoreView : MonoBehaviour
{
    private TMP_Text scoreText;

    private void Awake()
    {
        scoreText = GetComponent<TMP_Text>();
    }

    public void DisplayScore(int scoreAmount)
    {
        scoreText.text = scoreAmount.ToString();
    }
}
