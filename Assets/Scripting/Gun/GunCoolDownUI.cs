using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GunCooldownUI : MonoBehaviour
{
    [SerializeField] private Image bombIcon; // Assign in Inspector
    [SerializeField] private float cooldownTime = 3f;

    public void StartCooldown()
    {
        StartCoroutine(CooldownRoutine());
    }

   private IEnumerator CooldownRoutine()
    {
        float timer = 0f;
        while (timer < cooldownTime)
        {
            timer += Time.deltaTime;
            float grayscale = timer / cooldownTime; 
            bombIcon.color = Color.Lerp(Color.gray, Color.white, grayscale); // Gradually turn white
            yield return null;
        }
        bombIcon.color = Color.white;
    }

}
