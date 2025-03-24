using Unity.Mathematics;
using UnityEditor.Rendering;
using UnityEngine;

public class BallController : MonoBehaviour
{
  [SerializeField] private float speed = 10f;
  [SerializeField] private float lifetime = 5f;
  [SerializeField] private float damage = 10f;
  
  [SerializeField] private bool isP1Ball = true;
  

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
        if(isP1Ball){
            //Debug.Log("detected collision");
            if(collision.gameObject.CompareTag("TARGET")){
                //collision.gameObject.GetComponent<TargetController>().TakeDamage(damage);
            }
            if(collision.gameObject.CompareTag("TARGET2")){
                Debug.Log("Target Collision happened");
                collision.gameObject.GetComponent<TargetController>().TakeDamage(damage);
            }
            

        }
        if(!isP1Ball){
            if(collision.gameObject.CompareTag("TARGET")){
                Debug.Log("Target Collision happened");
                collision.gameObject.GetComponent<TargetController>().TakeDamage(damage);
            }
            if(collision.gameObject.CompareTag("TARGET2")){
                //collision.gameObject.GetComponent<TargetController>().TakeDamage(damage);
            }

        }
        
        
        // Vector2 reflectDir = Vector2.Reflect(rb.linearVelocity, collision.contacts[0].normal);
        // rb.linearVelocity = reflectDir.normalized * rb.linearVelocity.magnitude; // Maintain current speed
       
    }

    public static void SpawnProjectile(BallSo ballPrefab, Vector2 postion, Vector2 direction, float speed){
        GameObject ball = Instantiate(ballPrefab.Ball,postion,quaternion.identity);
        ball.GetComponent<BallController>().launch(speed,direction);
    }
    public static void SpawnAtOrigin(BallSo ballPrefab, float speed){
        Vector2 dir = UnityEngine.Random.insideUnitCircle.normalized;
        SpawnProjectile(ballPrefab,Vector2.zero,dir,speed);
    }
    

}
