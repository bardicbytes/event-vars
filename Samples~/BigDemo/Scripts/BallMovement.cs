using UnityEngine;

public class BallMovement : MonoBehaviour
{
    public Rigidbody body;
    public int seconds = 3;
    public float force = 2f;

    private float lastPush = 0;

    void FixedUpdate()
    {
        if (Time.time >= lastPush + seconds)
        {
            lastPush = Time.time;
            body.AddForce(new Vector3(Random.Range(-1f, 1f), Random.Range(0, 1f), Random.Range(-1f, 1f)) * force, ForceMode.VelocityChange);
        }
    }
}
