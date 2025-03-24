using UnityEngine;

public class TargetController : MonoBehaviour
{
    public float maxhealth = 100f;
    public float currentHealth;

    //1 or 2 to identify which player target it is
    public int playerID;
    [SerializeField] SpriteRenderer tempRend;
    

    
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

        if(currentHealth <=0)
        {
            //target Destroyed
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
    
}