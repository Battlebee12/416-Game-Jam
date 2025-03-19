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


    private void Update()
    {
        int movement_Int = inputManager.GetMovement2d();
        bool isJumping = inputManager.GetJumping2d();

        Vector3 move_dir = new Vector3(movement_Int,0,0);
        transform.position += move_dir * speed * Time.deltaTime;

        bool isGrounded = Physics2D.OverlapCircle(groundCheck.position,0.1f,groundLayer);
        if (isGrounded && isJumping){
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpforce);

        }
    }

}