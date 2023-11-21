﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crash : MonoBehaviour
{

    //private Animator animator;
    private int ship_HP = 3;
    private Rigidbody2D ship_rb;
    [SerializeField] private float crashTolerance;
    float speed;
    private bool checkingCrash;

    private int damageTimer = 5;
    private int currentDamageTimer = 5;

    private ShipControl shipControl;


    void Start()
    {
        ship_rb = gameObject.GetComponent<Rigidbody2D>();
        shipControl = gameObject.GetComponent<ShipControl>();
        //animator = gameObject.GetComponent<Animator>();
    }


    void FixedUpdate()
    {
        speed = ship_rb.velocity.magnitude;
        if (speed < 1) speed = 0;
        //Debug.Log("Speed is "+ speed );
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(speed > crashTolerance)
        {
            Debug.Log("Crashed");
            CheckCrash();
            //animator.SetBool("isCracked", true);
        }
    }

    private void CheckCrash()
    {
        if (checkingCrash) return;
        checkingCrash = true;

        ship_HP--;
        
        if (ship_HP > 0)
        {
            Debug.Log("Crashed Hull Integrity = " + ship_HP);
            TakeDamage();
            return;
        }
        if (ship_HP < 1)
        {
            Explode();
        }
    }   
    private void Explode()
    {
        GameEvents.current.Crash();
        Debug.Log("ALERT CATASTROPHIC DISASTER DETECTED");
    }

    private void TakeDamage()
    {
        StartCoroutine(TakeDamageCoroutine());
    }
    private IEnumerator TakeDamageCoroutine()
    {
        while (currentDamageTimer > 0)
        {
            shipControl.canControl = false;
            currentDamageTimer--;
            GameEvents.current.EngineOn();
            GameEvents.current.Yaw_R();
            yield return new WaitForSeconds(.5f);
        }
        
        shipControl.canControl = true;
        currentDamageTimer = damageTimer;
        checkingCrash = false;
        yield break;
        
    }
}
