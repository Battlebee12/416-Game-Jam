using UnityEngine;
using DG.Tweening;

public class CameraShake : MonoBehaviour
{
    private static CameraShake _instance;
    public static CameraShake Instance{
        get{
            if(_instance == null)
                _instance = Object.FindFirstObjectByType<CameraShake>();
            return _instance;
            }
        }
    
    public static void Shake(float duration, float strength){
        if(Instance != null){
            Instance.OnShake(duration,strength);
        }
    }
    private void OnShake(float duration, float strength){
        transform.DOShakePosition(duration,strength);
        transform.DOShakeRotation(duration,strength);
    }
}
