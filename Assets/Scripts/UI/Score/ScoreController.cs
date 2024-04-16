using System.Collections;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    public int Score { get; private set; }

    [SerializeField] private ScoreView scoreView;
    [SerializeField] private float smoothTime = 0.3f;

    private void OnEnable()
    {
        Block.OnOreCollected += AddScore;
    }

    private void OnDisable()
    {
        Block.OnOreCollected -= AddScore;
    }

    public void SetScoreView(ScoreView scoreView)
    {
        this.scoreView = scoreView;
    }

    private void AddScore(int scoreAmount)
    {
        StartCoroutine(SmoothAddScore(scoreAmount));
    }

    private IEnumerator SmoothAddScore(int scoreAmount)
    {
        int targetScore = Score + scoreAmount;

        while (Score < targetScore)
        {
            Score = Mathf.CeilToInt(Mathf.Lerp(Score, targetScore, smoothTime * Time.deltaTime));
            UpdateScoreView();
            yield return null;
        }

        Score = targetScore;
        UpdateScoreView();
    }

    private void UpdateScoreView()
    {
        scoreView.DisplayScore(Score);
    }
}
