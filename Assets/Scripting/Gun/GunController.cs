using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    [SerializeField] private int maxAmmo;

    [SerializeField] private float minTimeBtwShots;

    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Transform ballSpawnPostion;
        [SerializeField] private float speed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bool isShotting = inputManager.IsShooting();
        if (isShotting){
            BallController.SpawnProjectile(ballPrefab,ballSpawnPostion.position,gameObject.transform.right,speed);
            
        }

        
    }
}
