using Coffee.UIExtensions;
using System.Collections;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameMenuController: MonoBehaviour
{
    [SerializeField] private float showMenuDelay = 1f;
    [SerializeField] private float showNewHighScoreDelay = 0.1f;
    [SerializeField] private MenuPresenter menuPresenter;
    [SerializeField] private ScoreController playerScore;
    [SerializeField] private ScoreView highScoreView;
    [SerializeField] private ScoreView scoreView;
    [SerializeField] private ParticleSystem confettiParticle;
    [SerializeField] private GameObject leaderboardButton;


    private SoundManager soundManager;
    private GameManager gameManager;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        PlayerHealth.OnPlayerDie += HandleEndGameMenu;

        soundManager = SoundManager.instance;
        gameManager = GameManager.Instance;
    }

    private void OnDestroy()
    {
        PlayerHealth.OnPlayerDie -= HandleEndGameMenu;
    }

    public void PlaySwipeSound()
    {
        soundManager.Play("Swipe", false);
    }

    private void HandleEndGameMenu()
    {
        Invoke(nameof(ShowEndGameMenu), showMenuDelay);
    }

    private void ShowEndGameMenu()
    {
        animator.SetBool("IsMenuShowed", true);
        soundManager.Stop("CaveAmb");
    }

    public void HandleScoreDisplay()
    {
        playerScore.SetScoreView(scoreView);
        AccrueScore();

        if (gameManager.HighScore != 0)
        {
            highScoreView.DisplayScore(gameManager.HighScore);
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
            soundManager.Play("AddPoint", false);
            scoreView.DisplayScore(currentScore);
            yield return null;
        }

        currentScore = scoreAmount;
        scoreView.DisplayScore(currentScore);
        animator.SetTrigger("OnScoreShowed");
        soundManager.Play("Pop", true);
        ProcessNewHighScore();
        menuPresenter.ToggleMenuAndHud();
        menuPresenter.GetMenuView().SetContinueButtonVisibility(false);
        gameManager.ProcessRestartLevel();
    }

    private void ProcessNewHighScore()
    {
        if (playerScore.Score > GameManager.Instance.HighScore)
        {
            UpdateHighScore();
        }
    }

    private void UpdateHighScore()
    {
        gameManager.SetHighScoreAsPlayers();
        StartCoroutine(SetNewHighScore());
    }

    private IEnumerator SetNewHighScore()
    {
        yield return new WaitForSeconds(showNewHighScoreDelay);
        highScoreView.DisplayScore(playerScore.Score);
        soundManager.Play("Pop", true);
        Invoke(nameof(OnNewHighScore), 0.1f);
    }

    public void OnNewHighScore()
    {
        gameManager.CancelLevelRestart(SceneManager.GetActiveScene(), LoadSceneMode.Single);

        soundManager.Play("NewHighScore", false);
        confettiParticle.Play();
        leaderboardButton.SetActive(true);
    }
}