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

    public string[] roundScenes = { "Level1", "Level2", "Level3", "Level4" }; // Scene names
    private int currentRoundIndex = 0;


    protected override void Awake()
    {
        base.Awake(); // Call SingletonMonoBehavior's Awake()

        if (Instance != this)
        {
            return; // Singleton already set up
        }

        DontDestroyOnLoad(gameObject); // Make GameManager persist
        Debug.Log("GameManager Awake called");
    }

    void Start()
    {
        Debug.Log("GameManager Start called");
        InputManager[] inputManagers = FindObjectsByType<InputManager>(FindObjectsSortMode.None);
        RoundTimerUI[] timerUIArray = FindObjectsByType<RoundTimerUI>(FindObjectsSortMode.None);
        RoundTimerUI timerUI = timerUIArray.Length > 0 ? timerUIArray[0] : null;
        CalculateRoundWinner();
        roundUI.UpdateUIScore(player1Score, player2Score);
        FindTargets();
        roundTimerUI.OnTimerEnd.AddListener(EndRoundByTimer);
        roundEnded = false;
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

        Debug.Log("GameManager: Calling roundUI.EndRound()");
        roundUI.EndRound("Round Ended!");
        roundUI.UpdateUIScore(player1Score, player2Score);

        OnRoundEnded.Invoke();

        StartCoroutine(PrepareNextRound());
    }

    void EndRoundByTimer() 
    {
        if (!roundEnded) //Only call EndRound by the timer, if the game has not already ended.
        {
            EndRound();
        }
    }

    void CalculateRoundWinner()
    {
        Debug.Log("GameManager CalculateRoundWinner called");
        if (player1Target == null && player2Target != null)
        {
            Debug.Log("Player 2 wins by destroying Player 1's target!");
            player2Score++;
        }
        else if (player2Target == null && player1Target != null)
        {
            Debug.Log("Player 1 wins by destroying Player 2's target!");
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
            Debug.Log("Draw! Both targets are destroyed or don't exist.");
        }
    }

    // Coroutine to prepare for the next round after a short delay
    IEnumerator PrepareNextRound()
    {
        yield return new WaitForSeconds(2f); // Wait for 2 seconds
        
        // Load the next round scene
        LoadNextRound();
    }

    void FindTargets()
    {

        GameObject player1TargetObject = GameObject.FindWithTag("TARGET");
        GameObject player2TargetObject = GameObject.FindWithTag("TARGET2");

        if (player1TargetObject != null)
        {
            player1Target = player1TargetObject.GetComponent<TargetController>();
            if(player1Target != null){
                player1Target.OnTargetDestroyed.AddListener(() => {
                player1Target = null;
            });

            }
        }
        else
        {
            Debug.LogError("Target1 not found in the scene.");
        }

        if (player2TargetObject != null)
        {
            player2Target = player2TargetObject.GetComponent<TargetController>();
            if(player2Target != null){
                player2Target.OnTargetDestroyed.AddListener(() => {
                player2Target = null;
            });
        }
        }
        else
        {
            Debug.LogError("Target2 not found in the scene.");
        }

        if (player1Target != null)
        {
            player1Target.OnDamageTaken.AddListener(OnPlayerDamaged);
            player1Target.OnTargetDestroyed.AddListener(OnTargetDestroyed);
        }

        if (player2Target != null)
        {
            player2Target.OnDamageTaken.AddListener(OnPlayerDamaged);
            player2Target.OnTargetDestroyed.AddListener(OnTargetDestroyed);
        }
    }


    // This method will be called when either player's target takes damage
    public void OnPlayerDamaged(float damageAmount)
    {
        //possibly update for uI or sfx. scoring is in EndRound()

    }

    public void LoadNextRound()
    {
        currentRoundIndex++;
        if (currentRoundIndex < roundScenes.Length)
        {
            SceneManager.LoadScene(roundScenes[currentRoundIndex]);
            //roundTimerUI.SetNewRoundTime(60f); //Reset the timer
            InputManager[] inputManagers = FindObjectsByType<InputManager>(FindObjectsSortMode.None);
            foreach(InputManager manager in inputManagers){
                manager.SetCanShoot(true);
            }
            RoundTimerUI[] timerUIArray = FindObjectsByType<RoundTimerUI>(FindObjectsSortMode.None);
            if(timerUIArray.Length > 0){
                RoundTimerUI timerUI = timerUIArray[0];
                timerUI.SetNewRoundTime(60f);
            }
        }
        else
        {
            Debug.Log("Game Over! All rounds completed.");
            //space to make game over screen when all levels complete
        }
    }

    public void OnTargetDestroyed()
    {
        Debug.Log("GameManager: OnTargetDestroyed called. player1Target: " + (player1Target != null) + ", player2Target: " + (player2Target != null));
        if (!roundEnded)
        {
            roundEnded = true;
            CalculateRoundWinner();
            

            if(roundUI != null){
                roundUI.UpdateUIScore(player1Score,player2Score);
                string message = "Round Ended!";
                if(player1Target == null && player2Target != null){
                    message = "Player 2 wins!";
                }
                else if(player1Target != null && player2Target == null){
                    message = "Player 1 wins!";
                }else{
                    message = "It's a draw!";
                }
            roundUI.EndRound(message);

            InputManager[] inputManagers = FindObjectsByType<InputManager>(FindObjectsSortMode.None);
            foreach(InputManager manager in inputManagers){
                manager.SetCanShoot(false);
            }
            OnRoundEnded.Invoke();
            }
        }
        else
        {
            Debug.Log("GameManager: Round already ended.");
        }
    }

    // Unsubscribe from events when the object is destroyed
    private void OnDestroy()
    {
        if (player1Target != null)
        {
            player1Target.OnDamageTaken.RemoveListener(OnPlayerDamaged);
            if (player1Target.gameObject.GetComponent<TargetController>())
            {
                player1Target.gameObject.GetComponent<TargetController>().OnTargetDestroyed.RemoveListener(OnTargetDestroyed);
            }
        }
        if (player2Target != null)
        {
            player2Target.OnDamageTaken.RemoveListener(OnPlayerDamaged);
            if (player2Target.gameObject.GetComponent<TargetController>())
            {
                player2Target.gameObject.GetComponent<TargetController>().OnTargetDestroyed.RemoveListener(OnTargetDestroyed);
            }
        }
    }
}