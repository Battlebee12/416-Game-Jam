using UnityEngine;

public class MovingPlatforms : MonoBehaviour
{
    public float speed;
    public int Starting; //the initial position of the platform
    public Transform[] points;

    private int i = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.position = points[Starting].position;
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector2.Distance(transform.position,points[i].position) < 0.02f)
        {
            i++;
            if(i == points.Length)
            {
                i = 0;
            }
        }
    transform.position = Vector2.MoveTowards(transform.position, points[i].position, speed * Time.deltaTime);
} 
}