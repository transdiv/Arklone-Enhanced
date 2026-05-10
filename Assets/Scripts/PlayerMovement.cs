using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] TextMeshProUGUI score;
    [SerializeField] TextMeshProUGUI minimunTime;
    float bounds = 5.05f;

    private PlayerInput playerInput;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        GameManager.Instance.UpdateScore();
    }

    private void Move()
    {
        Vector2 input = playerInput.actions["Move"].ReadValue<Vector2>();
        float xInput = input.x;
        float newPositionX = transform.position.x + speed * xInput * Time.deltaTime;
        if (newPositionX < bounds && newPositionX > -bounds)
        {
            transform.position += new Vector3(speed * xInput * Time.deltaTime, 0f, 0f);
        }
    }
}
