using UnityEngine;
using UnityEngine.UI;
using System.Collections;

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

  
    void Start()
    {
        timer = roundTime;
        UpdateUI();
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            EndRound();
        }

        UpdateUI();
    }

    void EndRound()
    {
        timer = roundTime;
        //uncomment below after attaching to gameobjects

        //if (player1Target.GetCurrentHealth() < player2Target.GetCurrentHealth())
        //{
        //    player1Score++;
        //    Debug.Log("Player 1 wins the round!");
        //}
        //else if (player2Target.GetCurrentHealth() < player1Target.GetCurrentHealth())
        //{
        //    player2Score++;
        //    Debug.Log("Player 2 wins the round!");
        //}
        //else
        //{
        //    Debug.Log("Draw!");
        //}

        //player1Target.currentHealth = player1Target.maxHealth;
        //player2Target.currentHealth = player2Target.maxHealth;
        //player1Target.UpdateVisuals();
        //player2Target.UpdateVisuals();

        UpdateUI();
    }

    void UpdateUI()
    {
        timerText.text = "Time: " + Mathf.CeilToInt(timer);
        player1ScoreText.text = "Player 1: " + player1Score;
        player2ScoreText.text = "Player 2: " + player2Score;
    }
}