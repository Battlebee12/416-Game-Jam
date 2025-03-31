using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GunCooldownUI : MonoBehaviour
{
    [SerializeField] private Image bombIcon; // Assign in Inspector
    [SerializeField] private float cooldownTime = 3f; // Match GunController cooldown

    public void StartCooldown()
    {
        StartCoroutine(CooldownEffect());
    }

    private IEnumerator CooldownEffect()
    {
        float elapsed = 0f;
        Color initialColor = bombIcon.color;
        Color grayColor = new Color(0.1f, 0.1f, 0.1f, 1f); // Very dark gray

        bombIcon.color = grayColor;

        while (elapsed < cooldownTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / cooldownTime;
            bombIcon.color = Color.Lerp(grayColor, initialColor, t);
            yield return null;
        }

        // Make the icon noticeably bright for a moment
        bombIcon.color = Color.white;
        yield return new WaitForSeconds(0.3f); // Increased to 0.3s for visibility

        // Smooth transition back to normal
        float pulseDuration = 0.3f;
        float pulseElapsed = 0f;
        while (pulseElapsed < pulseDuration)
        {
            pulseElapsed += Time.deltaTime;
            bombIcon.color = Color.Lerp(Color.white, initialColor, pulseElapsed / pulseDuration);
            yield return null;
        }

        bombIcon.color = initialColor; // Ensure final color is restored
    }


}
