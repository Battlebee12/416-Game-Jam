using UnityEngine;

public class InputManager : MonoBehaviour
{

    [SerializeField] private bool isplayer2 = false;

    private KeyCode jump = KeyCode.W;
    private KeyCode shoot = KeyCode.F;
    private KeyCode right = KeyCode.D;
    private KeyCode left = KeyCode.A;

    private bool timerEnded = false;

    private void Start()
    {
        if(isplayer2){
            jump = KeyCode.UpArrow;
            shoot = KeyCode.RightShift;
            right = KeyCode.RightArrow;
            left = KeyCode.LeftArrow;
            
        }
        
    }
    public void SetTimerEnded(bool hasEnded){
        timerEnded = hasEnded;
    }

    public int GetMovement2d(){
        int movement_Int =0;
        if(Input.GetKey(left)){
            movement_Int = -1;
        }
        if(Input.GetKey(right)){
            movement_Int = 1;
        }
        return movement_Int;
    }

    public bool GetJumping2d(){
        return !timerEnded && Input.GetKeyDown(jump);
    }

    public bool IsShooting(){
        return !timerEnded && Input.GetKeyDown(shoot);
    }
}
