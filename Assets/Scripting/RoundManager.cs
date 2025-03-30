using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class RoundManagerUI : MonoBehaviour
{
    
    public TextMeshProUGUI player1ScoreText; 
    public TextMeshProUGUI player2ScoreText; 
    public GameObject roundEndDisplay;
    public TextMeshProUGUI roundEndText; 
    public Button nextRoundButton; // Changed button to nextRoundButton

    void Start()
    {
        roundEndDisplay.SetActive(false);
        nextRoundButton.onClick.AddListener(LoadNextRound); // Changed listener
        nextRoundButton.gameObject.SetActive(false); // hide until end of round
    }

    
    public void UpdateUIScore(int player1Score, int player2Score)
    {
        if (player1ScoreText != null)
        {
            player1ScoreText.text = "Player 1: " + player1Score;
        }
        if (player2ScoreText != null)
        {
            player2ScoreText.text = "Player 2: " + player2Score;
        }
    }

    public void EndRound(string message)
    {
        if (roundEndDisplay != null)
        {
            roundEndDisplay.SetActive(true);
            roundEndText.text = message;
            nextRoundButton.gameObject.SetActive(true); // show next round button
        }
    }

    public void LoadNextRound()
    {
        GameManager.Instance.LoadNextRound();
    }
}