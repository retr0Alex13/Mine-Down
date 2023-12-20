using UnityEngine;

public class ScoreController : MonoBehaviour
{
    public int Score { get; private set; }

    [SerializeField] private ScoreView scoreView;

    private void Awake()
    {
        Block.OnOreCollected += AddScore;
    }

    private void AddScore(int scoreAmount)
    {
        Score += scoreAmount;
        UpdateScoreView();
    }

    private void UpdateScoreView()
    {
        scoreView.UpdateScore(Score);
    }
}
