using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;


public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] TextMeshProUGUI score;
    [SerializeField] TextMeshProUGUI minimunTime;
    TimerHolder timeScript;
    float bounds = 5.05f;

    private void Start()
    {
        timeScript = GameObject.Find("TimerHolder").GetComponent<TimerHolder>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        score.text = string.Format(" {00000}", Math.Floor(timeScript.timer));
        minimunTime.text = string.Format(" {00000}", Math.Floor(timeScript.minimunTime));
    }

    private void Move()
    {
        float xInput = Input.GetAxisRaw("Horizontal");
        float newPositionX = transform.position.x + speed * xInput * Time.deltaTime;
        if (newPositionX < bounds && newPositionX > -bounds)
        {
            transform.position += new Vector3(speed * xInput * Time.deltaTime, 0f, 0f);
        }
    }
}
