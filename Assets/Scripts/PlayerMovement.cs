﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public static PlayerMovement player;
    public float thrustSpeed;

    public float pitch_speed, yaw_speed, roll_speed;

    public bool rotating = false;

    public float pitch, yaw, roll;
    Vector3 vel;

    public float accelRate, deccelTime, maxSpeed;
    public float acceleration = 0;

    // Dashing
    [SerializeField] float remainingDash = 0;
    public float dashAmount, lenthOfDash;

    Vector3 centerPos;

    ParticleSystem ps;

    Rigidbody rb;

	void Awake () {
        player = this;
        rb = GetComponent<Rigidbody>();
        ps = GetComponentInChildren<ParticleSystem>();
    }

    private void FixedUpdate()
    {
        if(Input.GetAxis("Thrust") != 0)
        {
            if (acceleration < maxSpeed)
            {
                acceleration += (accelRate * Time.deltaTime);
            }
        } else
        {
            if(acceleration > 0)
            {
                if(acceleration < 0.01f)
                {
                    acceleration = 0;
                } else
                {
                    acceleration = Mathf.Lerp(acceleration, 0, deccelTime * Time.deltaTime);
                }
            }
        }

        pitch = Input.GetAxis("Pitch") * pitch_speed;
        yaw = Input.GetAxis("Yaw") * yaw_speed;
        roll = Input.GetAxis("Roll") * roll_speed;

        transform.Rotate(pitch, yaw, roll);
        
        if(pitch == 0 && yaw == 0 && roll == 0)
        {
            rotating = true;
        } else
        {
            rotating = false;
        }

        // Dash
        if(Input.GetButtonDown("DashRight"))
        {
            if(remainingDash == 0)
            {
                remainingDash = dashAmount;
            }
        }
        if(Input.GetButtonDown("DashLeft"))
        {
            if(remainingDash == 0)
            {
                Debug.Log("HERE");
                remainingDash = -dashAmount;
            }
        }

        if (remainingDash == 0)
        {
            rb.MovePosition(rb.position + (transform.forward * acceleration));
        } else
        {
            Vector3 vDash = rb.position + (transform.right * (remainingDash / lenthOfDash));
            //Vector3 vDash = new Vector3(rb.position.x + (remainingDash / lenthOfDash), 0, 0);
            rb.MovePosition(vDash);
            if (remainingDash < 0)
            {
                remainingDash += dashAmount / lenthOfDash;
            } else
            {
                remainingDash -= dashAmount / lenthOfDash;
            }
        }
    }

    private void OnCollisionEnter(Collision coll)
    {
        if(coll.gameObject.tag == "Astro")
        {
            GameManager.Instance.Death();
        }
    }
}
