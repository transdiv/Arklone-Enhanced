using UnityEngine;

public class BallMovement : MonoBehaviour
{
    static Rigidbody2D rb;

    Vector2 launchForce;
    bool IsBallLaunched;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        launchForce = new Vector2(Random.Range(-240.0f, 240.0f), 375);
    }

    void Update()
    {
        if (Input.GetButtonDown("Launch") && !IsBallLaunched)
        {
            Launch();
        }
    }

    private void Launch()
    {
        rb.AddForce(launchForce);
        transform.parent = null;
        IsBallLaunched = true;
    }

    public static void StopBall()
    {
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
    }
}
