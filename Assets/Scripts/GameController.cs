using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    [SerializeField] private Text scoreLabel;
    [SerializeField] private Text resultScoreLabel;

    [SerializeField] private Text TimerLabel;

    [SerializeField] private Button PauseButton;
    [SerializeField] private Text PauseText;

    [SerializeField] private Button StartButton;
    [SerializeField] private Button RestartButton;

    [SerializeField] private GameObject GameOverPanel;

    [Space(10)]
    // 1 min 
    [SerializeField] private float GameTime = 60;
    [SerializeField] private float CreatingDelay = 0.3f;

    public static bool Pause = false;
    public static bool CanCreate = false;

    public float Timer
    {
        get
        {
            return timer;
        }
        set
        {
            timer = value;
            TimerLabel.text = "Time Left: " + (int)(GameTime - timer);
        }
    }
    private float timer;

    public int Score
    {
        get
        {
            return score;
        }
        set
        {
            score = value;
            scoreLabel.text = "Score: " + score;
        }
    }
    private int score;

    private void OnEnable()
    {
        Events.ChangeScore += ChangeScore;
        Events.GameOver += GameOver;

        StartButton.onClick.AddListener(
            ()=>
        {
            PauseButton.gameObject.SetActive(true);
            StartButton.gameObject.SetActive(false);
            Events.GameStart();
            StartCoroutine(IE_Timer());
        });

        RestartButton.onClick.AddListener(
            () =>
            {
                PauseButton.gameObject.SetActive(true);
                GameOverPanel.SetActive(false);
                Events.GameStart();
                StartCoroutine(IE_Timer());
            });
    }

    private void OnDestroy()
    {
        Events.ChangeScore -= ChangeScore;
        Events.GameOver -= GameOver;

        StartButton.onClick.RemoveListener(
            () =>
            {
                PauseButton.gameObject.SetActive(true);
                StartButton.gameObject.SetActive(false);
                Events.GameStart();
                StartCoroutine(IE_Timer());
            });

        RestartButton.onClick.RemoveListener(
            () =>
            {
                PauseButton.gameObject.SetActive(true);
                GameOverPanel.SetActive(false);
                Events.GameStart();
                StartCoroutine(IE_Timer());
            });
    }

    private void ChangeScore(int amount)
    {
        Score += amount;
    }

    public void SetPause()
    {
        Pause = !Pause;
        PauseText.text = Pause ? "Continue": "Pause";
    }

    private IEnumerator IE_Timer()
    {
        Timer = 0;
        Score = 0;
        CanCreate = false;

        float creatingTimer = 0;

        while (Timer < GameTime)
        {
            if(Timer >= creatingTimer)
            {
                creatingTimer += CreatingDelay;
                CanCreate = true;
            }

            yield return null;

            if(!Pause)
                Timer += Time.deltaTime;
        }

        Events.GameOver();
    }

    private void GameOver()
    {
        PauseButton.gameObject.SetActive(false);
        GameOverPanel.SetActive(true);
        resultScoreLabel.text = scoreLabel.text;
    }
}
