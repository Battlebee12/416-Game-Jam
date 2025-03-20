using Unity.Mathematics;
using UnityEditor.Rendering;
using UnityEngine;

public class BallController : MonoBehaviour
{
  [SerializeField] private float speed = 10f;
  [SerializeField] private float lifetime = 5f;

  [SerializeField] private Rigidbody2D rb;

    private void Start()
    {
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

    }
    private void launch(float speed, Vector2 direction){
        rb.linearVelocity =direction.normalized*speed;
        Destroy(gameObject,lifetime);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("detected collision");
        // Vector2 reflectDir = Vector2.Reflect(rb.linearVelocity, collision.contacts[0].normal);
        // rb.linearVelocity = reflectDir.normalized * rb.linearVelocity.magnitude; // Maintain current speed
       
    }

    public static void SpawnProjectile(GameObject ballPrefab, Vector2 postion, Vector2 direction, float speed){
        GameObject ball = Instantiate(ballPrefab,postion,quaternion.identity);
        ball.GetComponent<BallController>().launch(speed,direction);
    }
    public static void SpawnAtOrigin(GameObject ballPrefab, float speed){
        Vector2 dir = UnityEngine.Random.insideUnitCircle.normalized;
        SpawnProjectile(ballPrefab,Vector2.zero,dir,speed);
    }

}
