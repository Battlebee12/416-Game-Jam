using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;

public class GameManager : SingletonMonoBehavior<GameManager>
{
    
    public TargetController player1Target;
    public TargetController player2Target;

    public float roundTime = 60f;
    private float timer;

    public Text timerText;
    public Text player1ScoreText;
    public Text player2ScoreText;

    private int player1Score = 0;
    private int player2Score = 0;

    private bool roundEnded = false; // Flag to prevent multiple end-of-round calls

    void Start()
    {
        timer = roundTime;
        UpdateUI();

        // Subscribe to the OnDamageTaken events (optional, but could be used for other effects)
        if (player1Target != null)
        {
            player1Target.OnDamageTaken.AddListener(OnPlayerDamaged);
            // Also subscribe to the OnDestroy event to detect when a target is destroyed
            player1Target.gameObject.GetComponent<TargetController>().OnTargetDestroyed.AddListener(OnTargetDestroyed);
        }
        if (player2Target != null)
        {
            player2Target.OnDamageTaken.AddListener(OnPlayerDamaged);
            // Also subscribe to the OnDestroy event
            player2Target.gameObject.GetComponent<TargetController>().OnTargetDestroyed.AddListener(OnTargetDestroyed);
        }
    }

    void Update()
    {
        if (roundEnded) return; // Don't update timer if round has ended

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            EndRound();
        }

        UpdateUI();
    }

    void EndRound()
    {
        if (roundEnded) return;
        roundEnded = true;

        Debug.Log("Round Ended!");

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
            // Both targets are still alive, compare damage taken
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
                // You might want to handle draws differently, e.g., no score change
            }
        }
        else
        {
            Debug.Log("Draw! Both targets are destroyed or don't exist.");
        }

        // Reset for the next round
        StartCoroutine(PrepareNextRound());
    }

    // Coroutine to prepare for the next round after a short delay
    IEnumerator PrepareNextRound()
    {
        yield return new WaitForSeconds(2f); // Wait for 2 seconds

        roundEnded = false;
        timer = roundTime;

        // Re-initialize targets if they were destroyed
        if (player1Target == null)
        {
            // Instantiate or re-enable player 1 target here if needed
            Debug.Log("Re-initializing Player 1 target (placeholder)");
            // Example:(uncomment if this is plan)
            // player1Target = Instantiate(player1TargetPrefab, player1SpawnPoint.position, Quaternion.identity);
            // player1Target.OnDamageTaken.AddListener(OnPlayerDamaged);
            // player1Target.gameObject.GetComponent<TargetController>().OnTargetDestroyed.AddListener(OnTargetDestroyed);
        }
        else
        {
            player1Target.currentHealth = player1Target.maxhealth;
            player1Target.gameObject.SetActive(true); // Ensure it's active
            player1Target.UpdateVisuals();
        }

        if (player2Target == null)
        {
            // Instantiate or re-enable player 2 target here if needed
            Debug.Log("Re-initializing Player 2 target (placeholder)");
            // Example:(uncomment if this is plan)
            // player2Target = Instantiate(player2TargetPrefab, player2SpawnPoint.position, Quaternion.identity);
            // player2Target.OnDamageTaken.AddListener(OnPlayerDamaged);
            // player2Target.gameObject.GetComponent<TargetController>().OnTargetDestroyed.AddListener(OnTargetDestroyed);
        }
        else
        {
            player2Target.currentHealth = player2Target.maxhealth;
            player2Target.gameObject.SetActive(true); // Ensure it's active
            player2Target.UpdateVisuals();
        }

        UpdateUI();
    }

    // This method will be called when either player's target takes damage
    public void OnPlayerDamaged(float damageAmount)
    {
        //possibly update for uI or sfx. scoring is in EndRound()

        UpdateUI();
    }

    void UpdateUI()
    {
        timerText.text = "Time: " + Mathf.CeilToInt(timer);
        player1ScoreText.text = "Player 1: " + player1Score;
        player2ScoreText.text = "Player 2: " + player2Score;
    }

    public void OnTargetDestroyed(TargetController destroyedTarget)
    {
        if (!roundEnded)
        {
            EndRound(); // End the round immediately if a target is destroyed
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