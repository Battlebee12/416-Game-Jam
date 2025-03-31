using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : SingletonMonoBehavior<GameManager>
{
    public TargetController player1Target;
    public TargetController player2Target;

    private int player1Score = 0;
    private int player2Score = 0;
    private bool _roundEnded = false;

    public UnityEvent OnRoundEnded = new UnityEvent();
    public RoundManagerUI roundUI;
    public RoundTimerUI roundTimerUI;
    public Button nextRoundButton;

    public string[] roundScenes = { "Level_1_art", "Level_2_art", "Level_3_art", "Level_4_art" };
    private int currentRoundIndex = 0;

    protected override void Awake()
    {
        base.Awake();
        if (Instance != this) return;

        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded; // Ensure reinitialization after scene load
    }

    void Start()
    {
        Debug.Log("GameManager Start called");
        init();
        FindTargets();
        roundTimerUI.OnTimerEnd.AddListener(EndRoundByTimer);
        roundEnded = false;
    }

    private void init()
    {
        Debug.Log("Initializing GameManager...");

        player1Target = GameObject.Find("Target1")?.GetComponent<TargetController>();
        player2Target = GameObject.Find("Target2")?.GetComponent<TargetController>();
        roundUI = GameObject.Find("RoundEndUI (1)")?.GetComponent<RoundManagerUI>();
        roundTimerUI = GameObject.Find("TimerDisplayCanvas")?.GetComponent<RoundTimerUI>();

        

        nextRoundButton = FindInactiveButtonByName("NextRound");

        if (nextRoundButton != null)
            Debug.Log("Next Round Button found successfully!");

        if (roundUI != null) roundUI.UpdateUIScore(player1Score, player2Score);
    }
    private Button FindInactiveButtonByName(string buttonName)
    {
        Button[] allButtons = Resources.FindObjectsOfTypeAll<Button>();
        foreach (Button btn in allButtons)
        {
            if (btn.gameObject.name == buttonName)
            {
                return btn;
            }
        }
        return null;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Scene Loaded: {scene.name}");
        init();  // Reinitialize GameManager references
        FindTargets();
        roundTimerUI.SetNewRoundTime(60f); // Reset the timer
        roundTimerUI.OnTimerEnd.RemoveListener(EndRoundByTimer);
        roundTimerUI.OnTimerEnd.AddListener(EndRoundByTimer);
    }

    public bool roundEnded
    {
        get { return _roundEnded; }
        private set { _roundEnded = value; }
    }

    void EndRound()
    {
        roundEnded = true;
        Debug.Log("GameManager: EndRound called. roundEnded: " + roundEnded);

        CalculateRoundWinner();
        roundUI?.EndRound("Round Ended!");
        roundUI?.UpdateUIScore(player1Score, player2Score);

        OnRoundEnded.Invoke();
        //StartCoroutine(PrepareNextRound());
    }

    void EndRoundByTimer()
    {
        if (!roundEnded) EndRound();
    }

    void CalculateRoundWinner()
    {
        Debug.Log("GameManager CalculateRoundWinner called");
        if (player1Target == null && player2Target != null)
        {
            Debug.Log("Player 2 wins!");
            player2Score++;
        }
        else if (player2Target == null && player1Target != null)
        {
            Debug.Log("Player 1 wins!");
            player1Score++;
        }
        else if (player1Target != null && player2Target != null)
        {
            float player1DamageTaken = player1Target.maxhealth - player1Target.currentHealth;
            float player2DamageTaken = player2Target.maxhealth - player2Target.currentHealth;

            if (player1DamageTaken < player2DamageTaken)
            {
                Debug.Log("Player 1 wins by taking less damage!");
                player1Score++;
            }
            else if (player2DamageTaken < player1DamageTaken)
            {
                Debug.Log("Player 2 wins by taking less damage!");
                player2Score++;
            }
            else
            {
                Debug.Log("Draw! Both players took the same amount of damage.");
            }
        }
        else
        {
            Debug.Log("Draw! Both targets are destroyed.");
        }
    }

    IEnumerator PrepareNextRound()
    {
        yield return new WaitForSeconds(2f);
        LoadNextRound();
    }

    void FindTargets()
    {
        Debug.Log("Finding targets...");
        GameObject player1TargetObject = GameObject.FindWithTag("TARGET");
        GameObject player2TargetObject = GameObject.FindWithTag("TARGET2");

        player1Target = player1TargetObject?.GetComponent<TargetController>();
        player2Target = player2TargetObject?.GetComponent<TargetController>();

        if (player1Target != null)
        {
            player1Target.OnTargetDestroyed.AddListener(() => player1Target = null);
            player1Target.OnDamageTaken.AddListener(OnPlayerDamaged);
            player1Target.OnTargetDestroyed.AddListener(OnTargetDestroyed);
        }

        if (player2Target != null)
        {
            player2Target.OnTargetDestroyed.AddListener(() => player2Target = null);
            player2Target.OnDamageTaken.AddListener(OnPlayerDamaged);
            player2Target.OnTargetDestroyed.AddListener(OnTargetDestroyed);
        }
    }

    public void OnPlayerDamaged(float damageAmount)
    {
        // Possibly update UI or play sound effects
    }

    public void LoadNextRound()
    {
        roundEnded = false;
        currentRoundIndex++;
        if (SceneManager.GetActiveScene().buildIndex <5)
        {
             Destroy(roundTimerUI.gameObject);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            Destroy(roundTimerUI.gameObject);
            Debug.Log("Game Over! All rounds completed.");
            SceneManager.LoadScene(0);
        }
    }

    public void OnTargetDestroyed()
    {
        if (!roundEnded)
        {
            roundEnded = true;
            CalculateRoundWinner();

            if (roundUI != null)
            {
                roundUI.UpdateUIScore(player1Score, player2Score);
                string message = player1Target == null && player2Target != null ? "Player 2 wins!" :
                                 player2Target == null && player1Target != null ? "Player 1 wins!" :
                                 "It's a draw!";
                roundUI.EndRound(message);
            }

            foreach (InputManager manager in FindObjectsByType<InputManager>(FindObjectsSortMode.None))
            {
                manager.SetCanShoot(false);
            }
            OnRoundEnded.Invoke();
        }
        else
        {
            Debug.Log("GameManager: Round already ended.");
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        if (player1Target != null)
        {
            player1Target.OnDamageTaken.RemoveListener(OnPlayerDamaged);
            player1Target.OnTargetDestroyed.RemoveListener(OnTargetDestroyed);
        }
        if (player2Target != null)
        {
            player2Target.OnDamageTaken.RemoveListener(OnPlayerDamaged);
            player2Target.OnTargetDestroyed.RemoveListener(OnTargetDestroyed);
        }
    }
}
