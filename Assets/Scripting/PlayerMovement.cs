using System.Collections;
using UnityEditor.Callbacks;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private int speed;
    [SerializeField] private float jumpforce = 5f;
    [SerializeField] private InputManager inputManager;

    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private Animator animator;

    [SerializeField] private Transform playerVisual;
    private Vector3 local_scale;

    private void Start(){
        local_scale = new Vector3(playerVisual.localScale.x, playerVisual.localScale.y, playerVisual.localScale.z);
    }


    private void Update()
    {
        int movement_Int = inputManager.GetMovement2d();
        bool isJumping = inputManager.GetJumping2d();

        Vector3 move_dir = new Vector3(movement_Int,0,0);
        transform.position += move_dir * speed * Time.deltaTime;

        animator.SetFloat("SPEED", Mathf.Abs(movement_Int));

       
        if (movement_Int > 0) 
        {
            playerVisual.localScale = new Vector3(local_scale.x,local_scale.y,local_scale.z);
        }
        else if (movement_Int < 0) 
        {
            playerVisual.localScale = new Vector3(-local_scale.x,local_scale.y,local_scale.z);
        }

        bool isGrounded = Physics2D.OverlapCircle(groundCheck.position,0.35f,groundLayer);
        if (isGrounded && isJumping){
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpforce);
            animator.SetBool("J", true);

        }
        else if(isGrounded){
            animator.SetBool("J", false); 


        }
        bool is_attacking = inputManager.IsShooting();
        if (is_attacking){
            // animator.SetBool("A",true);
            animator.SetTrigger("ATTACK");
           // StartCoroutine(ResetTrigger());
        }

    public void OnCollisionEnter2D(Collision2D collision){
            if(collision.gameObject.CompareTag("TARGET")){
                    Debug.Log("Target Collision happened");
                    collision.gameObject.GetComponent<TargetController>().TakeDamage(damage);
                    Destroy(gameObject);
                }
    }

    }
    private IEnumerator ResetTrigger()
    {
        yield return new WaitForSeconds(0.3f); // Wait for 0.3 seconds
        animator.ResetTrigger("ATTACK");
    }

}