using System.Collections;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    [SerializeField] private float minTimeBtwShots = 3f; // Time delay between shots
    
    [SerializeField] private BallSo ballSO;
    [SerializeField] private Transform ballSpawnPostion;
    [SerializeField] private float speed;
    [SerializeField] private GunCooldownUI cooldownUI;
    
    private bool canShoot = true;
    private float lastShotTime = 0f;
    public AudioClip shootSound;

    void Update()
    {
        if (canShoot && inputManager.IsShooting() && Time.time - lastShotTime >= minTimeBtwShots)
        {
            lastShotTime = Time.time;
            AudioManager.Instance.PlaySFX(shootSound);
            StartCoroutine(Fire());
            cooldownUI.StartCooldown(); 
        }
    }

    private IEnumerator Fire()
    {
        canShoot = false;
        yield return new WaitForSeconds(0.15f); // Delay before firing
        BallController.SpawnProjectile(ballSO, ballSpawnPostion.position, transform.right, speed);
        canShoot = true;
    }
}
