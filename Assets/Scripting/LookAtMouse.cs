
using UnityEngine;

public class LookAtMouse : MonoBehaviour
{
    [SerializeField] private Transform gameObj;
    [SerializeField] private float rotationSpeed;
    bool flipped = false;
    
    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = -(transform.position - mousePosition);

        direction.z =0;

        float targetAngle = Mathf.Atan2(direction.y, direction.x)*Mathf.Rad2Deg;
        float angle = Mathf.LerpAngle(gameObj.eulerAngles.z,targetAngle,rotationSpeed*Time.deltaTime);

        bool shouldFlip = targetAngle > 90f || targetAngle < -90f;
        if(shouldFlip && !flipped){
            Debug.Log("Flip ran");
            flip(shouldFlip);
            flipped = true;
            
        }
        if(!shouldFlip && flipped){
            Debug.Log("Flip ran");
            flip(shouldFlip);
            flipped = false;
        }

        // Apply the rotation
        gameObj.rotation = Quaternion.Euler(new Vector3(gameObj.rotation.x, 0, angle));
        
        
    }
    private void flip(bool shouldFlip){
          Vector3 scale = transform.localScale;

        // Flip the GameObject by multiplying the x component of the scale by -1
        scale.y *= -1;

        // Apply the new scale to the GameObject
        transform.localScale = scale;
    }
}
