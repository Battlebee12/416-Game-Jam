using UnityEngine;

public class InvisibleWall : MonoBehaviour
{
    private void OnTriggerEnter(Collider o){
        if(o.CompareTag("Player")){
             Vector3 push = new Vector3(o.transform.position.x,o.transform.position.y-1,o.transform.position.z);
             o.transform.position = push;
        }
    }

    
}
