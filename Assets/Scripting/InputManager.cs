using UnityEngine;

public class InputManager : MonoBehaviour
{
    public int GetMovement2d(){
        int movement_Int =0;
        if(Input.GetKey(KeyCode.A)){
            movement_Int = -1;
        }
        if(Input.GetKey(KeyCode.D)){
            movement_Int = 1;
        }
        return movement_Int;
    }

    public bool GetJumping2d(){
        return Input.GetKey(KeyCode.Space);
    }

    public bool IsShooting(){
        return Input.GetKeyDown(KeyCode.F);
    }
}
