using UnityEngine;
using UnityEngine.Events;

public class TargetController : MonoBehaviour
{
    public float maxhealth = 100f;
    public float currentHealth;

    //1 or 2 to identify which player target it is
    public int playerID;
    [SerializeField] SpriteRenderer tempRend;

    // Create a UnityEvent that takes a float (the amount of damage taken)
    public UnityEvent<float> OnDamageTaken;
    // Create a UnityEvent that is invoked when the target is destroyed
    public UnityEvent<TargetController> OnTargetDestroyed; // Pass the TargetController itself

    void Start()
    {
        //set health to max at start of round
        currentHealth = maxhealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxhealth); //prevent negative health

        //Visual feedback (colour change?)
        UpdateVisuals();

        // Invoke the UnityEvent, passing the damage amount
        OnDamageTaken?.Invoke(damage);

        if (currentHealth <= 0)
        {
            // Target Destroyed
            OnTargetDestroyed?.Invoke(this); // Invoke the OnTargetDestroyed event, passing this instance
            Destroy(gameObject);
        }
    }
    public void UpdateVisuals()
    {
        //change colour based on health %
        float healthPercentage = currentHealth / maxhealth;
        tempRend.material.color = Color.Lerp(Color.red, Color.green, healthPercentage);
    }

    private void UpdateVisualsInternal() // Kept private
    {
        float healthPercentage = currentHealth / maxhealth;
        tempRend.material.color = Color.Lerp(Color.red, Color.green, healthPercentage);
    }

    //getter for health
    public float getCurrentHealth()
    {
        return currentHealth;
    }
    
}