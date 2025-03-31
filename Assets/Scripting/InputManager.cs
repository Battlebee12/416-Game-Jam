using UnityEngine;

public class InputManager : MonoBehaviour
{

    [SerializeField] private bool isplayer2 = false;

    private KeyCode jump = KeyCode.W;
    private KeyCode shoot = KeyCode.F;
    private KeyCode right = KeyCode.D;
    private KeyCode left = KeyCode.A;

    private bool timerEnded = false;
    private bool canShoot = true;

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

    public void SetCanShoot(bool value){
        canShoot = value;
    }

    public int GetMovement2d(){
        if(!canShoot) return 0;
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
        return canShoot && !timerEnded && Input.GetKeyDown(jump);
    }

    public bool IsShooting(){
        return canShoot && !timerEnded && Input.GetKeyDown(shoot);
    }
}
