using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    Rigidbody2D rb;
    Animator animator;
    PlayerControl playerControl;

    public KeyCode punch;
    public KeyCode ultimate;
    
    [Header("Attack Configuration")]
    public bool isAttack;
    public float attackStrength = 20;
    public float attackSpeed = 1f;
    public float stuntDelay = 0.2f;
    public GameObject atkHitbox;

    [Header("Ultimate Configuration")]
    public int ultimatePoint = 0;
    public int ultimateCharge = 100;
    public bool ultimateStatus;

    bool isKnockback;
    UnityEngine.Vector2 knockbackForce;
    GameObject hand;

    void Start(){
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        atkHitbox.SetActive(false);
        playerControl = GetComponent<PlayerControl>();
        hand = playerControl.Hand;
    }

    void Update(){
        // Punch Function
        if(UnityEngine.Input.GetKeyDown(punch) && !isAttack){
            attack();
        }

        // Ultimate Skill Function
        if(ultimatePoint >= ultimateCharge){
            ultimatePoint = ultimateCharge;
            ultimateStatus = true;
        }
        if(UnityEngine.Input.GetKeyDown(ultimate)){
            if(ultimateStatus){
                ultimatePoint = 0;
                ultimateStatus = false;
                Debug.Log($"ULTIMATE {transform.name} ! ! !");
            }
            else Debug.Log($"{transform.name} not ready for ultimate");
        }
    }

    void FixedUpdate(){
        if(isKnockback){
            rb.AddForce(knockbackForce, ForceMode2D.Impulse);
            isKnockback = false;
        }
    }
    
    private void attack(){
        hand.SetActive(false);
        isAttack = true;
        animator.Play("punch");
        atkHitbox.SetActive(true);
        Invoke("resetAttack", 0.4f/attackSpeed);
    }
    private void resetAttack() {
        hand.SetActive(true);
        isAttack = false;
        atkHitbox.SetActive(false);
        animator.Play("idle");
    }
    public void hurt(Transform attacker, float strength){
        animator.Play("hurt");
        playerControl.canMove = false;
        knockbackForce = (transform.position - attacker.transform.position).normalized*strength;
        isKnockback = true;
        ultimatePoint += 1;

        Invoke("resetHurt", stuntDelay);
    }

    private void resetHurt() {
        //rb.velocity = Vector3.zero;
        playerControl.canMove = true;
        animator.Play("idle");
    }
}