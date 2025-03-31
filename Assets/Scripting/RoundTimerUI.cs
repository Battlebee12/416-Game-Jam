using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class RoundTimerUI : MonoBehaviour
{
    public float roundTime = 5f; // Total round time
    private float timer;
    public TextMeshProUGUI timerText;
    private bool timerEnded = false;

    public UnityEvent OnTimerEnd = new UnityEvent();

    void Start()
    {
        timer = roundTime;
        Debug.Log("RoundTimerUI: timerText assigned in Start(). timerText: " + (timerText != null));
        UpdateTimerDisplay();
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(timerText.gameObject);
    }

    void Update()
    {
        if (timerEnded) return;

        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = 0; // Prevent negative time
            timerEnded = true;
            OnTimerEnd?.Invoke();
            InputManager[] inputManagers = FindObjectsByType<InputManager>(FindObjectsSortMode.None);
            foreach(InputManager manager in inputManagers){
                manager.SetTimerEnded(true);
            }
        }
        UpdateTimerDisplay();
    }

    void UpdateTimerDisplay()
    {
        Debug.Log("UpdateTimerDisplay called. timerText: " + (timerText != null)); // Add this line
        if (timerText != null)
        {
            timerText.text = "Time: " + Mathf.CeilToInt(timer);
        }
    }

    public void SetNewRoundTime(float newRoundTime)
    {
        roundTime = newRoundTime;
        timer = roundTime;
        timerEnded = false;
        UpdateTimerDisplay();
    }
}