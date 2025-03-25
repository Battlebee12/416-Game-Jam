using System.Collections;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    [SerializeField] private int maxAmmo = 1;
    [SerializeField] private float duration = 5f;
    [SerializeField] private float minTimeBtwShots;

    [SerializeField] private BallSo ballSO;
    [SerializeField] private Transform ballSpawnPostion;
    [SerializeField] private float speed;

    private bool coolDown = false;
    private float coolDownTimer = 0f;

    // Update is called once per frame
    void Update()
    {
        if(coolDown){
            coolDownTimer -= Time.deltaTime;
            if(coolDownTimer < 0){
                coolDown = false;
                maxAmmo = 3;
            }
        }
        else{
        bool isShooting = inputManager.IsShooting();
        if (isShooting && maxAmmo > 0){
            StartCoroutine(ResetTrigger());
            //BallController.SpawnProjectile(ballSO,ballSpawnPostion.position,gameObject.transform.right,speed);
            maxAmmo--;
        }
            if(maxAmmo <= 0){
                coolDown = true;
                coolDownTimer = duration;
            }
        }
    }
     private IEnumerator ResetTrigger()
    {
        yield return new WaitForSeconds(0.15f); // Wait for 0.3 seconds
        BallController.SpawnProjectile(ballSO,ballSpawnPostion.position,gameObject.transform.right,speed);
        
    }
}
