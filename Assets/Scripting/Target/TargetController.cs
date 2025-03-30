using UnityEngine;
using UnityEngine.Events;

public class TargetController : MonoBehaviour
{
    public float maxhealth = 100f;
    public float currentHealth;

    //1 or 2 to identify which player target it is
    public int playerID;
    [SerializeField] SpriteRenderer tempRend;

    public UnityEvent<float> OnDamageTaken;
    public UnityEvent OnTargetDestroyed;

    private bool targetDestroyedInvoked = false; // Flag to prevent multiple invocations

    void Start()
    {
        //set health to max at start of round
        currentHealth = maxhealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxhealth); //prevent negative health

        //Visual feedback 
        UpdateVisuals();

        OnDamageTaken?.Invoke(damage);

        if (currentHealth <=0 && !targetDestroyedInvoked)
        {
            Debug.Log("Target " + gameObject.name + " destroyed. Invoking OnTargetDestroyed.");
            OnTargetDestroyed?.Invoke();
            targetDestroyedInvoked = true; // Set the flag
            Destroy(gameObject);
        }
    }
    void UpdateVisuals()
    {
        //change colour based on health %
        float healthPercentage = currentHealth / maxhealth;
        tempRend.material.color = Color.Lerp(Color.red, Color.green, healthPercentage);
    }

    //getter for health
    public float getCurrentHealth()
    {
        return currentHealth;
    }

    // Unsubscribe from events when the object is destroyed
    private void OnDestroy()
    {
        // Check if OnTargetDestroyed has any listeners before removing them
        if (OnTargetDestroyed != null)
        {
            OnTargetDestroyed.RemoveAllListeners();
        }

        if (OnDamageTaken != null)
        {
            OnDamageTaken.RemoveAllListeners();
        }
    }

}