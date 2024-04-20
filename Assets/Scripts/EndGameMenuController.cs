using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class EndGameMenuController : MonoBehaviour
{
    [SerializeField] private float showMenuDelay = 1f;
    [SerializeField] private float showNewHighScoreDelay = 0.1f;
    [SerializeField] private MenuPresenter menuPresenter;
    [SerializeField] private ScoreController playerScore;
    [SerializeField] private ScoreView highScoreView;
    [SerializeField] private ScoreView scoreView;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        PlayerHealth.OnPlayerDie += HandleEndGameMenu;
    }

    private void OnDestroy()
    {
        PlayerHealth.OnPlayerDie -= HandleEndGameMenu;
    }

    private void HandleEndGameMenu()
    {
        StartCoroutine(ShowEndGameMenu());
    }

    private IEnumerator ShowEndGameMenu()
    {
        yield return new WaitForSeconds(showMenuDelay);
        animator.SetBool("IsMenuShowed", true);
        SoundManager.instance.Stop("CaveAmb");
    }

    public void SetScoresText()
    {
        playerScore.SetScoreView(scoreView);
        AccrueScore();
        if (GameManager.Instance.HighScore != 0)
        {
            highScoreView.DisplayScore(GameManager.Instance.HighScore);
        }
    }

    private void AccrueScore()
    {
        StartCoroutine(SmoothAddScore(playerScore.Score));
    }

    private IEnumerator SmoothAddScore(int scoreAmount)
    {
        int currentScore = 0;

        while (currentScore < scoreAmount)
        {
            currentScore = Mathf.CeilToInt(Mathf.Lerp(currentScore, scoreAmount, 30f * Time.deltaTime));
            SoundManager.instance.Play("AddPoint", false);
            scoreView.DisplayScore(currentScore);
            yield return null;
        }

        currentScore = scoreAmount;
        scoreView.DisplayScore(currentScore);
        animator.SetTrigger("OnScoreShowed");
        ProcessNewHighScore();
    }

    private void ProcessNewHighScore()
    {
        if (playerScore.Score > GameManager.Instance.HighScore)
        {
            GameManager.Instance.SetHighScoreAsPlayers();
            StartCoroutine(SetNewHighScore());
            menuPresenter.ToggleMenuAndHud();
            menuPresenter.GetMenuView().SetContinueButtonVisibility(false);
            GameManager.Instance.ProcessRestartLevel();
        }
    }

    private IEnumerator SetNewHighScore()
    {
        yield return new WaitForSeconds(showNewHighScoreDelay);
        highScoreView.DisplayScore(playerScore.Score);
        SoundManager.instance.Play("HighScore", true);
    }
}