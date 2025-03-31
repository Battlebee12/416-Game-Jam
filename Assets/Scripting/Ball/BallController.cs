using Unity.Mathematics;
using UnityEditor.Rendering;
using UnityEngine;
using System.Collections;

public class BallController : MonoBehaviour
{
  [SerializeField] private float speed = 5f;
  [SerializeField] private float lifetime = 5f;
  [SerializeField] private float damage = 10f;
  [SerializeField] private bool isP1Ball = true;
  [SerializeField] private ParticleSystem particlesDestroy;
  [SerializeField] private ParticleSystem particlesTimer;
  public AudioClip exploSound;

  [SerializeField] private Rigidbody2D rb;

    private void Start()
    {
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

    }
    private void launch(float speed, Vector2 direction){
        rb.linearVelocity =direction.normalized*speed;
        StartCoroutine(HandleLifeTime());
       // Destroy(gameObject,lifetime);

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
                AudioManager.Instance.PlaySFX(exploSound);
                CameraShake.Shake(0.45f,0.75f);
                ActivateParticles();
            }

            if(collision.gameObject.CompareTag("Spike")){
                Debug.Log("Spike destroyed ball!");
                AudioManager.Instance.PlaySFX(exploSound);
                ActivateParticles();
            }
            

        }
        if(!isP1Ball){
            if(collision.gameObject.CompareTag("TARGET")){
                Debug.Log("Target Collision happened");
                collision.gameObject.GetComponent<TargetController>().TakeDamage(damage);
                AudioManager.Instance.PlaySFX(exploSound);
                CameraShake.Shake(0.45f,0.75f);
                ActivateParticles();
            }
            if(collision.gameObject.CompareTag("TARGET2")){
                //collision.gameObject.GetComponent<TargetController>().TakeDamage(damage);
            }

            if(collision.gameObject.CompareTag("Spike")){
                Debug.Log("Spike destroyed ball!");
                AudioManager.Instance.PlaySFX(exploSound);
                ActivateParticles();
            }

        }
        // Vector2 reflectDir = Vector2.Reflect(rb.linearVelocity, collision.contacts[0].normal);
        // rb.linearVelocity = reflectDir.normalized * rb.linearVelocity.magnitude; // Maintain current speed
    }

    public static void SpawnProjectile(BallSo ballPrefab, Vector2 postion, Vector2 direction, float speed){
        GameObject ball = Instantiate(ballPrefab.Ball,postion,quaternion.identity);
        ball.tag = "Ball";
        ball.layer = LayerMask.NameToLayer("Ball");
        ball.GetComponent<BallController>().launch(speed,direction);
        Debug.Log("Spawned Ball:" + ball.tag);
    }
    public static void SpawnAtOrigin(BallSo ballPrefab, float speed){
        Vector2 dir = UnityEngine.Random.insideUnitCircle.normalized;
        SpawnProjectile(ballPrefab,Vector2.zero,dir,speed);
    }
    
    private void ActivateParticles(){
            if(particlesDestroy != null){
                ParticleSystem particles = Instantiate(particlesDestroy,transform.position,Quaternion.identity);
                particles.Play();
                Destroy(particles.gameObject,particles.main.duration);
            }
            Destroy(gameObject);
        }

    private IEnumerator HandleLifeTime(){
        yield return new WaitForSeconds(lifetime);
        if(particlesTimer != null){
                ParticleSystem timer = Instantiate(particlesTimer,transform.position,Quaternion.identity);
                AudioManager.Instance.PlaySFX(exploSound);
                timer.Play();
                Destroy(timer.gameObject,timer.main.duration);
            }
            Destroy(gameObject);
    }

}
