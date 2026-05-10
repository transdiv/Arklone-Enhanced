using UnityEngine;
using UnityEngine.InputSystem;

public class BallMovement : MonoBehaviour
{
    [SerializeField] AudioClip ballTilt;

    Rigidbody2D rb;

    Vector2 launchForce;
    bool IsBallLaunched;

    float tiltCooldown = 3f;
    float nextTiltTime;

    private PlayerInput playerInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        launchForce = new Vector2(Random.Range(-240.0f, 240.0f), 375);
    }

    void Update()
    {
        bool launch = playerInput.actions["Launch"].WasPressedThisFrame();
        if (launch && !IsBallLaunched)
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

    public void StopBall()
    {
        if (rb == null) return;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
    }

    

    public void Tilt()
    {
        if (Time.time < nextTiltTime)
            return;
        nextTiltTime = Time.time + tiltCooldown;
        // A more moderate tilt:
        // Random.Range(-1f, 1f),
        // Random.Range(-0.3f, 0.3f)
        Vector2 randomOffset = new Vector2(
               Random.Range(-1.2f, 1.2f),
               Random.Range(-1.0f, 1.0f)
           );
        Vector2 newDirection = (rb.linearVelocity.normalized + randomOffset).normalized;
        float speed = rb.linearVelocity.magnitude;
        rb.linearVelocity = newDirection * speed;
        Camera.main.GetComponent<CameraShake>().Shake(0.15f, 0.1f);
        GameManager.Instance.PlaySoundEffect(ballTilt, 0.5f);
    }

}
